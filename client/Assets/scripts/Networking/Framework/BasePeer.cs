using System;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Networking.Framework.Dispatchers;
using Networking.Framework.Utils;
using Networking.Protobuf.CommunicationProtocol;
using Debug = UnityEngine.Debug;
using Logger = Networking.Framework.Utils.Logger;

namespace Networking.Framework
{
    /// <summary>
    /// Peer which communicates with master server via google protocol buffers
    /// </summary>
    public abstract class BasePeer : INetworkPeer, IDisposable
    {
        public bool IsConnected => _connection.IsEstablished;

        /// <summary>
        /// <see cref="ImmediateDispatcher"/> will be used by default so messages from remote server can be handled
        /// in background thread. Pass <see cref="IDispatcher"/> in overload constructor to override this behaviour.
        /// </summary>
        /// <param name="userData"></param>
        public BasePeer(UserData userData) :
            this(new ImmediateDispatcher(), userData)
        {
        }

        /// <param name="commandsDispatcher">Dispatcher for handling commands received from remote server</param>
        public BasePeer(IDispatcher commandsDispatcher, UserData userData)
        {
            _userData = userData;
            _commandsDispatcher = commandsDispatcher ?? throw new ArgumentNullException(nameof(commandsDispatcher));
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task BroadcastEventAsync(int eventCode)
        {
            await BroadcastEventAsync(null, eventCode);
        }

        public async Task BroadcastEventAsync(byte[] serializedEvent, int eventCode)
        {
            var eventDataBase64 = serializedEvent == null ? string.Empty : Convert.ToBase64String(serializedEvent, 0, serializedEvent.Length);

            var customEvent = new CustomEventData()
            {
                Code = eventCode,
                Payload = ByteString.FromBase64(eventDataBase64)
            };

            var message = new EventData()
            {
                Code = EventCode.CustomEvent,
                Payload = customEvent.ToByteString()
            };

            Logger.Log($"Broadcast event {eventCode}", NetworkLogType.Broadcasting);
            await SendRequestAsync(message: message, OperationRequestCode.RaiseEvent);
        }

        public async Task ConnectWithMasterServerAsync()
        {
            if (_connection != null && _connection.IsEstablished)
            {
                Logger.LogError("Already connected");
                return;
            }

            _connection?.Dispose();
            _connection = new Connection(DataHandler, ConnectionInfo);
            await _connection.InitAsync(async () => { await SendHandshakeAsync(); });
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
        private readonly UserData _userData;

        private void DataHandler(byte[] data)
        {
            Command command;
            try
            {
                command = Command.Parser.ParseFrom(data);
            }
            catch (Exception e)
            {
                Logger.LogError($"Exception occured while parsing command, received unexpected data: [{Encoding.ASCII.GetString(data)}]");
                Debug.LogException(e);
                return;
            }

            Logger.Log($"Received command [{command.Type}] [{command.CalculateSize()}B]", NetworkLogType.Broadcasting);
            _commandsDispatcher.Invoke(() => HandleCommand(command));
        }

        private async Task SendRequestAsync(IMessage message, OperationRequestCode code)
        {
            var request = new OperationRequest()
            {
                RequestCode = code,
                Payload = message.ToByteString(),
            };

            Logger.Log($"Sending {CommandType.OpRequest} [{request.RequestCode}]", NetworkLogType.Broadcasting);
            await SendCommandAsync(CommandType.OpRequest, request);
        }

        private async Task SendCommandAsync(CommandType type, IMessage payload)
        {
            if (!_connection.IsEstablished)
            {
                //TODO: add queue buffer
                Logger.LogError("Failed to send command, disconnected with host.");
                return;
            }

            var command = new Command()
            {
                Type = type,
                Payload = payload.ToByteString()
            };

            Logger.Log($"Sent command [{type}] [{command.CalculateSize()}B]", NetworkLogType.Broadcasting);
            await Task.Run(() => command.WriteTo(_connection.Stream));
        }

        private async Task SendHandshakeAsync()
        {
            var handshake = new HandshakePayload()
            {
                ProtocolVersion = Config.ProtocolVersion,
                User = new Protobuf.CommunicationProtocol.UserData()
                {
                    Username = _userData.Login
                }
            };

            await SendRequestAsync(handshake, OperationRequestCode.Handshake);
        }

        private void HandleCommand(Command command)
        {
            switch (command.Type)
            {
                case CommandType.OpResponse:
                    var response = OperationResponse.Parser.ParseFrom(command.Payload);
                    Logger.Log($"Received response [{response.ResponseCode}]", NetworkLogType.Broadcasting);
                    HandleResponse(response);
                    break;
                case CommandType.Event:
                    var eventData = EventData.Parser.ParseFrom(command.Payload);
                    Logger.Log($"Received event [{eventData.Code}]", NetworkLogType.Broadcasting);
                    HandleEvent(eventData);
                    break;
                default:
                    throw new NotSupportedException(command.Type.ToString());
            }
        }
    }
}