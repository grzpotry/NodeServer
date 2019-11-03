﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.Protobuf.CommunicationProtocol;
using Google.Protobuf;
using UnityEngine;

namespace Domain
{
    public class NetworkClient : INetworkClient
    {
        public event Action<INetworkClient> Connected;
        public event Action<INetworkClient> Disconnected;
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

        public void Update()
        {
            //TODO: move to another thread
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

        private async Task SendRequestAsync(IMessage message, OperationRequestCode code)
        {
            var request = new OperationRequest()
            {
                RequestCode = code,
                Payload = message.ToByteString(),
            };

            Debug.Log($"Sending {CommandType.OpRequest} [{request.RequestCode}]");
            await SendCommandAsync(CommandType.OpRequest, request);
        }

        private async Task SendCommandAsync(CommandType type, IMessage payload)
        {
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

        private async Task HandleCommandAsync(Command command)
        {
            switch (command.Type)
            {
                case CommandType.OpResponse:
                    var response = OperationResponse.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received response [{response.ResponseCode}]");
                    break;
                case CommandType.Event:
                    var eventData = EventData.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received event [{eventData.Code}]");
                    break;
                default:
                    throw new NotSupportedException(command.Type.ToString());
            }
        }
    }
}