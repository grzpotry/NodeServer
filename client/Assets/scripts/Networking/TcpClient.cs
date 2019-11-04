using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    /// <summary>
    /// Wraps <see cref="System.Net.Sockets.TcpClient"/>
    /// </summary>
    public class TcpClient : NetworkClient, IDisposable
    {
        public override NetworkStream NetworkStream => _client.GetStream();
        public override bool Connected
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

        readonly byte[] _connectionTestBuffer = new byte[1];

        public void Dispose()
        {
            Disconnect(reuseSocket: false);
        }

        public override async Task ConnectAsync()
        {
            if (Connected)
            {
                throw new InvalidOperationException();
            }
            _client = new System.Net.Sockets.TcpClient();
            await _client.ConnectAsync(_info.IpAddress, _info.Port);
            Debug.Log($"Connected with {_info.IpAddress}:{_info.Port}");
        }

        public override async Task ReconnectAsync()
        {
            Disconnect(reuseSocket: true);
            await ConnectAsync();
        }

        public override void Disconnect(bool reuseSocket)
        {
            if (_client.Connected)
            {
                _client.Client.Disconnect(reuseSocket);
            }
            _client.Close();
            _client.Dispose();
        }

        private System.Net.Sockets.TcpClient _client;
        private readonly ConnectionInfo _info;
        private readonly object _connectedLock = new object();
    }
}