using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace Domain
{
    public class Client : IClient
    {
        public Client(bool autoReconnect)
        {
            _autoReconnect = autoReconnect;
        }

        public event Action<IClient> Connected;
        public event Action<IClient> Disconnected;

        public bool IsConnected => _socket?.IsAlive ?? false;

        public void Dispose()
        {
            if (_socket == null)
            {
                return;
            }

            _socket.OnOpen -= OnConnected;
            _socket.OnClose -= OnDisconnected;
            _socket.Close();
            ((IDisposable) _socket)?.Dispose();
            _socket = null;
        }

        public void Update()
        {
            if (_autoReconnect && _connectionLost && Time.realtimeSinceStartup - _lastReconnectionTime > ReconnectIntevalSeconds)
            {
                _lastReconnectionTime = Time.realtimeSinceStartup;
                ConnectWithHost(_adress, _port);
            }
        }

        public void ConnectWithHost(string adress, int port)
        {
            _port = port;
            _adress = adress;
            _socket = GetOrCreateSocket(adress, port);

            if (_socket.IsAlive)
            {
                Debug.LogError("Already connected");
                return;
            }

            _socket.ConnectAsync();
        }

        private const int ReconnectIntevalSeconds = 1;
        private readonly bool _autoReconnect;
        private WebSocket _socket;
        private string _adress;
        private int _port;
        private bool _connectionLost;
        private float _lastReconnectionTime;

        private WebSocket GetOrCreateSocket(string adress, int port)
        {
            if (_socket != null)
            {
                return _socket;
            }

            var socket = new WebSocket($"ws://{adress}:{port}/socket.io/?EIO=2&transport=websocket");
            socket.OnOpen += OnConnected;
            socket.OnClose += OnDisconnected;
            return socket;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Debug.Log("Connected");
            _connectionLost = false;
            Connected?.Invoke(this);
        }

        private void OnDisconnected(object sender, CloseEventArgs e)
        {
            Debug.Log("Disconnected");
            _connectionLost = true;
            Disconnected?.Invoke(this);
        }
    }
}