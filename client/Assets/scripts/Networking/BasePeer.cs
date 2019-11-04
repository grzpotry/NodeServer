using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Protobuf.CommunicationProtocol;
using Google.Protobuf;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BasePeer : INetworkPeer, IDisposable
    {
        public bool IsConnected => _connection.IsEstablished;

        public void Dispose()
        {
            _connection.Dispose();
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
            if (_connection != null && _connection.IsEstablished)
            {
                Debug.LogError("Already connected");
                return;
            }

            _connection?.Dispose();
            _connection = new Connection(DataHandler, ConnectionInfo);
            await _connection.InitAsync();
            await SendHandshakeAsync();
        }

        protected abstract ConnectionInfo ConnectionInfo { get; }
        protected abstract Task HandleResponseAsync(OperationResponse response);
        protected abstract Task HandleEventAsync(EventData eventData);

        private Connection _connection;

        private async Task DataHandler(byte[] data)
        {
            Command command;
            try
            {
                command = Command.Parser.ParseFrom(data);
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occured while parsing command, received unexpected data: [{Encoding.ASCII.GetString(data)}]");
                Debug.LogException(e);
                return;
            }

            Debug.Log($"Received command [{command.Type}] [{command.CalculateSize()}B]");
            await HandleCommandAsync(command);
        }

        private async Task SendCommandAsync(CommandType type, IMessage payload)
        {
            if (!_connection.IsEstablished)
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
            await Task.Run(() => command.WriteTo(_connection.Stream));
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