using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Networking.Framework.Utils;
using Debug = UnityEngine.Debug;

namespace Networking.Framework
{
    /// <summary>
    /// Network connection with master server established through TCP protocol
    /// </summary>
    public class Connection : IDisposable
    {
        /// <summary>
        /// Delegate for handling data received from network
        /// </summary>
        /// <param name="data"></param>
        public delegate void DataHandler(byte[] data);

        public bool IsEstablished => _client.Connected;
        public NetworkStream Stream => _client.NetworkStream;

        public Connection(DataHandler receivedDataHandler, ConnectionInfo info)
        {
            _receivedDataHandler = receivedDataHandler ?? throw new ArgumentNullException(nameof(receivedDataHandler));
            _info = info;
            _client = new TcpClient(_info);
        }

        public async Task InitAsync(Func<Task> handshake)
        {
            if (_isInited)
            {
                throw new InvalidOperationException("Connection was already initialized.");
            }

            _handshake = handshake ?? throw new ArgumentNullException(nameof(handshake));
            _timer.Start();

            await _client.ConnectAsync();

            _updateThread = new Thread(Update)
            {
                IsBackground = true,
                Name = $"Connection with {_info.IpAddress}"
            };

            _cancelUpdateTokenSource = new CancellationTokenSource();
            _updateThread.Start();
            _isInited = true;

            await handshake();
        }

        public void Dispose()
        {
            Logger.Log("Connection disposed");
            _isInited = false;
            _cancelUpdateTokenSource?.Cancel();

            if (_updateThread != null && _updateThread.IsAlive)
            {
                Logger.LogError("Aborting update thread");
                _updateThread.Abort();
            }

            _client.Dispose();
            _cancelUpdateTokenSource?.Dispose();
        }

        private readonly Stopwatch _timer = new Stopwatch();
        private readonly TcpClient _client;
        private readonly DataHandler _receivedDataHandler;
        private readonly ConnectionInfo _info;
        private readonly byte[] _dataBundleBuffer = new byte[1024];

        private bool _isInited;
        private float _lastReconnectionTimeSeconds;
        private CancellationTokenSource _cancelUpdateTokenSource;

        private Thread _updateThread;
        private Func<Task> _handshake;

        private long LifeTimeMs => _timer.ElapsedMilliseconds;

        private async Task ReadAsync()
        {
            var stream = _client.NetworkStream;
            if (!stream.CanRead)
            {
                throw new Exception("Can't read from network stream");
            }

            if (!stream.DataAvailable)
            {
                return;
            }

            var data = await ReadData(stream);
            _receivedDataHandler(data);
        }

        private async Task<byte[]> ReadData(NetworkStream stream)
        {
            using (var tempDataBundle = ListPool<byte>.Create())
            {
                while (stream.DataAvailable)
                {
                    int receivedBytes = await stream.ReadAsync(_dataBundleBuffer, 0, _dataBundleBuffer.Length);
                    tempDataBundle.Value.AddRange(_dataBundleBuffer.Take(receivedBytes));
                }

                return tempDataBundle.Value.ToArray();
            }
        }

        private async void Update()
        {
            while (!_cancelUpdateTokenSource.IsCancellationRequested)
            {
                if (!_client.Connected && LifeTimeMs - _lastReconnectionTimeSeconds > Config.ReconnectIntevalMs)
                {
                    _lastReconnectionTimeSeconds = LifeTimeMs;

                    try
                    {
                        Logger.LogWarning("Attempting reconnect");
                        await _client.ReconnectAsync(_cancelUpdateTokenSource);
                        await _handshake();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                if (!IsEstablished)
                {
                    continue;
                }

                try
                {
                    await ReadAsync();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}