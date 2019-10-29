using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Networking.Protobuf.CommunicationProtocol;
using Google.Protobuf;
using UnityEngine;

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

        public async Task SendHandshakeAsync()
        {
            var handshake = new HandshakePayload()
            {
                ProtocolVersion = 1
            };

            await SendRequestAsync(handshake, OperationRequestCode.Handshake);
        }

        private async Task SendRequestAsync(IMessage message, OperationRequestCode code)
        {
            var request = new OperationRequest()
            {
                RequestCode = code,
                Payload = message.ToByteString(),
            };
            await SendCommandAsync(CommandType.OpRequest, request);
        }

        private async Task SendCommandAsync(CommandType type, IMessage payload)
        {
            var command = new Command()
            {
                Type = type,
                Payload = payload.ToByteString()
            };

            Debug.Log($"Sent command {type} Payload size: {command.CalculateSize()} Payload size: {payload.CalculateSize()}");
            await Task.Run(() => command.WriteTo(_stream));
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


            //TODO: deserialize protobuf message
            //TODO: Resolve incoming command basing on first byte
           var firstByte =  _bundle.First();
           Debug.LogError("FIRST BYTE " + firstByte);
           if (firstByte == (byte)CommandType.OpResponse)
           {
               Debug.LogError("Response");
           }


            var message = Encoding.ASCII.GetString(_bundle.ToArray()); //TODO: very inefficient allocation
            Debug.Log($"Received total {_bundle.Count} bytes message: {message}");
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
            await SendHandshakeAsync();
        }

        private const int ReconnectIntevalSeconds = 1;

        private string _adress;
        private int _port;
        private float _lastReconnectionTime;
        private NetworkStream _stream;
        private TcpClient _tcpClient = new TcpClient();

        private readonly List<byte> _bundle = new List<byte>();
        private readonly byte[] _bundleBuffer = new byte[1024];
    }
}