﻿using System;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Networking.Framework.Dispatchers;
using Networking.Protobuf.CommunicationProtocol;
using Debug = UnityEngine.Debug;

namespace Networking.Framework
{
    /// <summary>
    /// Peer which communicates with host via google protocol buffers
    /// </summary>
    public abstract class BasePeer : INetworkPeer, IDisposable
    {
        public bool IsConnected => _connection.IsEstablished;

        /// <summary>
        /// <see cref="ImmediateDispatcher"/> will be used by default so messages from remote server can be handled
        /// in background thread. Pass <see cref="IDispatcher"/> in overload constructor to override this behaviour.
        /// </summary>
        public BasePeer() :
            this(new ImmediateDispatcher())
        {
        }

        /// <param name="commandsDispatcher">Dispatcher for handling commands received from remote server</param>
        public BasePeer(IDispatcher commandsDispatcher)
        {
            _commandsDispatcher = commandsDispatcher ?? throw new ArgumentNullException(nameof(commandsDispatcher));
        }

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

        /// <summary>
        /// This method is called on background thread, so you cannot use unity-specific features here
        /// </summary>
        protected abstract void HandleResponse(OperationResponse response);

        /// <summary>
        /// This method is called on background thread, so you cannot use unity-specific features here
        /// </summary>
        protected abstract void HandleEvent(EventData eventData);

        private Connection _connection;
        private readonly IDispatcher _commandsDispatcher;

        private void DataHandler(byte[] data)
        {
            Command command;
            try
            {
                command = Command.Parser.ParseFrom(data);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"Exception occured while parsing command, received unexpected data: [{Encoding.ASCII.GetString(data)}]");
                Debug.LogException(e);
                return;
            }

            Debug.Log($"Received command [{command.Type}] [{command.CalculateSize()}B]");
            _commandsDispatcher.Invoke(() => HandleCommand(command));
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

        private void HandleCommand(Command command)
        {
            switch (command.Type)
            {
                case CommandType.OpResponse:
                    var response = OperationResponse.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received response [{response.ResponseCode}]");
                    HandleResponse(response);
                    break;
                case CommandType.Event:
                    var eventData = EventData.Parser.ParseFrom(command.Payload);
                    Debug.Log($"Received event [{eventData.Code}]");
                    HandleEvent(eventData);
                    break;
                default:
                    throw new NotSupportedException(command.Type.ToString());
            }
        }
    }
}