using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Networking.Protobuf.CommunicationProtocol;
using Google.Protobuf;
using Debug = UnityEngine.Debug;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseNetworkClient : INetworkClient, IDisposable
    {
        public event Action<INetworkClient> Connected;
        public event Action<INetworkClient> Disconnected;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public void Dispose()
        {
            if (_tcpClient == null)
            {
                return;
            }

            _cancelConnectionUpdateTokenSource.Dispose();
            _tcpClient.Client.Disconnect(reuseSocket: false);
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = null;
        }

        private void UpdateConnection()
        {
            while (!_cancelConnectionUpdateTokenSource.IsCancellationRequested)
            {
                if (!IsConnected && SecondsFromFirstConnection - _lastReconnectionTime > ReconnectIntevalSeconds)
                {
                    _lastReconnectionTime = SecondsFromFirstConnection;

                    try
                    {
                        Debug.Log("Attempting reconnection");
                        ConnectWithHostAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                if (IsConnected)
                {
                    try
                    {
                        ReadMessagesAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

        }

        public async Task SendRequestAsync(IMessage message, OperationRequestCode code)
        {
            var request = new OperationRequest()
            {
                RequestCode = code,
                Payload = message.ToByteString(),
            };

            Debug.Log($"Sending {CommandType.OpRequest} [{request.RequestCode}]");
            await SendCommandAsync(CommandType.OpRequest, request);
        }

        public async Task ConnectWithHostAsync()
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                Debug.LogError("Already connected");
                return;
            }

            _tcpClient?.Close();
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ConnectionInfo.IpAddress, ConnectionInfo.Port);
            _stream = _tcpClient.GetStream();

            Debug.Log("Connected with host");

            if (!_connectionTimer.IsRunning)
            {
                _connectionTimer.Start();
            }

            var connectionUpdateThread = new Thread(UpdateConnection)
            {
                IsBackground = true,
                Name = "Host connection"
            };
            _cancelConnectionUpdateTokenSource = new CancellationTokenSource();
            connectionUpdateThread.Start();

            await SendHandshakeAsync();
        }

        private long SecondsFromFirstConnection => _connectionTimer.ElapsedMilliseconds / 1000;

        protected abstract ConnectionInfo ConnectionInfo { get; }
        protected abstract Task HandleResponseAsync(OperationResponse response);
        protected abstract Task HandleEventAsync(EventData eventData);

        private const int ReconnectIntevalSeconds = 1;
        private float _lastReconnectionTime;
        private NetworkStream _stream;
        private TcpClient _tcpClient;
        private CancellationTokenSource _cancelConnectionUpdateTokenSource;

        private Stopwatch _connectionTimer = new Stopwatch();

        private readonly List<byte> _bundle = new List<byte>();
        private readonly byte[] _bundleBuffer = new byte[1024];

        private async Task SendCommandAsync(CommandType type, IMessage payload)
        {
            if (!IsConnected)
            {
                //TODO: add queue buffer
                Debug.LogError("Failed to send command, disconnected with host.");
                return;
            }

            var command = new Command()
            {
                Type = type,
                Payload = payload.ToByteString()
            };

            Debug.Log($"Sent command [{type}] [{command.CalculateSize()}B]");
            await Task.Run(() => command.WriteTo(_stream));
        }

        private async Task ReadMessagesAsync()
        {
            if (!_stream.CanRead)
            {
                throw new Exception("Can't read from network stream");
            }

            if (!_stream.DataAvailable)
            {
                return;
            }

            _bundle.Clear();

            while (_stream.DataAvailable)
            {
                int receivedBytes = await _stream.ReadAsync(_bundleBuffer, 0, _bundleBuffer.Length);
                _bundle.AddRange(_bundleBuffer.Take(receivedBytes));
            }

            Command command;
            try
            {
                command = Command.Parser.ParseFrom(_bundle.ToArray());
            }
            catch (Exception e)
            {
                Debug.LogError("Received unexpected data, exception occured while parsing command.");
                Debug.LogException(e);
                return;
            }

            Debug.Log($"Received command [{command.Type}] [{command.CalculateSize()}B]");
            await HandleCommandAsync(command);
        }

        private async Task SendHandshakeAsync()
        {
            var handshake = new HandshakePayload()
            {
                ProtocolVersion = 1
            };

            await SendRequestAsync(handshake, OperationRequestCode.Handshake);
        }

        private async Task HandleCommandAsync(Command command)
        {
            switch (command.Type)
            {
                case CommandType.OpResponse:
                    var response = OperationResponse.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received response [{response.ResponseCode}]");
                    await HandleResponseAsync(response);
                    break;
                case CommandType.Event:
                    var eventData = EventData.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received event [{eventData.Code}]");
                    await HandleEventAsync(eventData);
                    break;
                default:
                    throw new NotSupportedException(command.Type.ToString());
            }
        }
    }
}