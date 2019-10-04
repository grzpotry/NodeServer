﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Dispose()
        {
            if (_tcpClient == null)
            {
                return;
            }

            _tcpClient.Client.Disconnect(reuseSocket: false);
            _tcpClient.Close();
            _tcpClient.Dispose();
            _tcpClient = null;
        }

        public void Update()
        {
            if (!IsConnected && Time.realtimeSinceStartup - _lastReconnectionTime > ReconnectIntevalSeconds)
            {
                _lastReconnectionTime = Time.realtimeSinceStartup;

                try
                {
                    ConnectWithHostAsync(_adress, _port);
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

            var message = Encoding.ASCII.GetString(_bundle.ToArray()); //TODO: very inefficient allocation
            Debug.Log($"Received total {_bundle.Count} bytes message: {message}");
        }

        public async Task SendMessageAsync(string message)
        {
            if (!_tcpClient.Connected)
            {
                throw new Exception("Not connected with server");
            }
            byte[] outStream = Encoding.ASCII.GetBytes(message);

            if (!_stream.CanWrite)
            {
                throw new Exception("Can't write to server stream");
            }

            await _stream.WriteAsync(outStream, 0, outStream.Length);
        }

        public async Task ConnectWithHostAsync(string adress, int port)
        {
            _port = port;
            _adress = adress;

            if (_tcpClient.Connected)
            {
                Debug.LogError("Already connected");
                return;
            }

            await _tcpClient.ConnectAsync(adress, port);
            _stream = _tcpClient.GetStream();
            await HandshakeAsync();
        }

        private async Task HandshakeAsync()
        {
            await SendMessageAsync("Hello, playerXXX here");
        }

        private const int ReconnectIntevalSeconds = 1;

        private string _adress;
        private int _port;
        private float _lastReconnectionTime;
        private NetworkStream _stream;
        private TcpClient _tcpClient = new TcpClient();

        private readonly List<byte> _bundle = new List<byte>();
        private readonly byte[] _bundleBuffer = new byte[1024]; //just single package which we
    }
}