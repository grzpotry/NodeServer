using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace Domain
{
    public class Client : IClient
    {
        public event Action<IClient> Connected;
        public event Action<IClient> Disconnected;
        public bool IsConnected => _tcpClient.Connected;

        public Client(bool autoReconnect)
        {
            _autoReconnect = autoReconnect;
        }

        public void Dispose()
        {
            if (_tcpClient == null)
            {
                return;
            }

            _shouldBeConnected = false;
            _tcpClient.Client.Disconnect(reuseSocket: false);
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = null;
        }

        public async Task SendMessageAsync(string message)
        {
            if (!_tcpClient.Connected)
            {
                throw new Exception("Not connected with server");
            }
            byte[] outStream = Encoding.ASCII.GetBytes(message);
            var stream = _tcpClient.GetStream();
            if (!stream.CanWrite)
            {
                throw new Exception("Can't write to server stream");
            }

            await stream.WriteAsync(outStream, 0, outStream.Length);
        }

        public void Update()
        {
            var lostConnection = _shouldBeConnected && !IsConnected;

            if (_autoReconnect && lostConnection && Time.realtimeSinceStartup - _lastReconnectionTime > ReconnectIntevalSeconds)
            {
                _lastReconnectionTime = Time.realtimeSinceStartup;
                ConnectWithHost(_adress, _port);
            }
        }

        public void ConnectWithHost(string adress, int port)
        {
            _port = port;
            _adress = adress;

            if (_tcpClient.Connected)
            {
                Debug.LogError("Already connected");
                return;
            }

            _tcpClient.ConnectAsync(adress, port);
            _shouldBeConnected = true;
        }

        private const int ReconnectIntevalSeconds = 1;
        private readonly bool _autoReconnect;
        private string _adress;
        private int _port;
        private bool _shouldBeConnected;
        private float _lastReconnectionTime;
        private TcpClient _tcpClient = new TcpClient();
    }
}