using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Networking.Framework.Utils;
using UnityEngine;

namespace Networking.Framework
{
    /// <summary>
    /// Wraps <see cref="System.Net.Sockets.TcpClient"/>
    /// </summary>
    public class TcpClient : IDisposable, INetworkClient
    {
        public NetworkStream NetworkStream => _client.GetStream();

        public bool Connected
        {
            get
            {
                if (_client?.Client == null || !_client.Connected)
                {
                    return false;
                }

                try
                {
                    if (_client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        return _client.Client.Receive(_connectionTestBuffer, SocketFlags.Peek) != 0;
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        public TcpClient(ConnectionInfo info)
        {
            _info = info;
        }

        public void Dispose()
        {
            Disconnect(reuseSocket: false);
        }

        public Task ConnectAsync()
        {
            return ConnectAsyncInternal(null);
        }

        public Task ConnectAsync(CancellationTokenSource cancelTokenSource)
        {
            return ConnectAsyncInternal(cancelTokenSource);
        }

        public async Task ReconnectAsync()
        {
            Disconnect(reuseSocket: true);
            await ConnectAsyncInternal();
        }

        public async Task ReconnectAsync(CancellationTokenSource cancelTokenSource)
        {
            Disconnect(reuseSocket: true);
            await ConnectAsyncInternal(cancelTokenSource);
        }

        public void Disconnect(bool reuseSocket)
        {
            if (_client.Connected)
            {
                _client.Client.Disconnect(reuseSocket);
            }

            _client.Close();
            _client.Dispose();
        }

        private readonly ConnectionInfo _info;
        private readonly byte[] _connectionTestBuffer = new byte[1];
        private System.Net.Sockets.TcpClient _client;

        private async Task ConnectAsyncInternal(CancellationTokenSource cts = null)
        {
            if (Connected)
            {
                throw new InvalidOperationException();
            }

            _client = new System.Net.Sockets.TcpClient();
            if (cts == null)
            {
                await _client.ConnectAsync(_info.IpAddress, _info.Port);
            }
            else
            {
                try
                {
                    await _client.ConnectAsync(_info.IpAddress, _info.Port).WithCancellation(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning("Connecting interrupted");
                    return;
                }
            }

            Debug.Log($"Connected with {_info.IpAddress}:{_info.Port}");
        }
    }
}