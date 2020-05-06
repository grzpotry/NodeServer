using System;
using System.Linq;
using System.Net;
using System.Text;
using Networking.Framework;
using Networking.Framework.Dispatchers;
using Networking.Framework.Utils;
using Networking.Protobuf.CommunicationProtocol;
using Newtonsoft.Json;
using Simulation.Domain.Network.Data;
using Simulation.Domain.Serialization;
using UnityEngine;
using Logger = Networking.Framework.Utils.Logger;
using UserData = Networking.Framework.UserData;

namespace Simulation.Domain.Network
{
    /// <summary>
    /// Custom peer
    /// </summary>
    public class Peer : ThreadSafePeer
    {
        public Peer(IUnityThreadListener unityListener, UserData userData, Simulation simulation, bool isHost)
            : base(unityListener, userData)
        {
            _isHost = isHost;
            _simulation = simulation ?? throw new ArgumentNullException(nameof(simulation));
        }

        protected override ConnectionInfo ConnectionInfo { get; } = new ConnectionInfo(IPAddress.Parse(Config.ServerAdress), Config.Port);

        protected override void HandleResponse(OperationResponse response)
        {
            switch (response.ResponseCode)
            {
                case OperationResponseCode.InvalidProtocol:
                case OperationResponseCode.InvalidLogin:
                case OperationResponseCode.InvalidPassword:
                    Logger.LogError(response.ResponseCode.ToString());
                    break;
                case OperationResponseCode.HandshakeSuccess:
                    Logger.Log(response.ResponseCode.ToString());
                    break;
                case OperationResponseCode.AlreadyConnected:
                    Logger.LogWarning(response.ResponseCode.ToString());
                    break;
            }
        }

        protected override void HandleEvent(EventData eventData)
        {
            switch (eventData.Code)
            {
                case EventCode.ClientJoined:
                    var joinedUser = Networking.Protobuf.CommunicationProtocol.UserData.Parser.ParseFrom(eventData.Payload);
                    if (_simulation.Players.Any(_ => _.Id == joinedUser.Username))
                    {
                        Debug.LogWarning($"{joinedUser.Username} is already in simulation");
                        return;
                    }

                    var player = new Player(islocalPlayer: false, joinedUser.Username);
                    _simulation.AddPlayer(player);
                    Debug.Log($"User {joinedUser.Username} joined server");
                    break;
                case EventCode.ClientLeft:
                    var leavingUser = Networking.Protobuf.CommunicationProtocol.UserData.Parser.ParseFrom(eventData.Payload);
                    Debug.Log($"User {leavingUser.Username} left server");
                    _simulation.RemovePlayer(id: leavingUser.Username);
                    break;
                case EventCode.CustomEvent:
                    var customEventData = CustomEventData.Parser.ParseFrom(eventData.Payload);
                    HandleCustomEvent(customEventData);
                    break;
            }
        }

        private readonly Simulation _simulation;
        private readonly bool _isHost;

        private async void HandleCustomEvent(CustomEventData eventData)
        {
            try
            {
                Logger.Log("Handled custom event " + eventData.Code, NetworkLogType.Broadcasting);

                var code = (CustomEventCode) eventData.Code;

                switch (code)
                {
                    case CustomEventCode.PlayerMoved:

                        var movedEvent = DeserializeCustomEvent<PlayerMovedEventDTO>(eventData);

                        var player = _simulation.Players.FirstOrDefault(_ => _.Id == movedEvent.PlayerId);

                        if (player == null)
                        {
                            Debug.LogError($"Player {movedEvent.PlayerId} not found ");
                            break;
                        }

                        player.MoveTo(movedEvent.Position);
                        break;

                    case CustomEventCode.FullUpdateRequest:
                        if (!_isHost)
                        {
                            break;
                        }

                        var fullUpdateEvent = new FullUpdateEventDTO()
                        {
                            Simulation = _simulation.Save()
                        };

                        var json = JsonConvert.SerializeObject(fullUpdateEvent, new Vector3Converter());
                        var bytes = Encoding.UTF8.GetBytes(json);
                        await BroadcastEventAsync(bytes, (int) CustomEventCode.FullUpdate);
                        break;
                    case CustomEventCode.FullUpdate:
                        var updateEvent = DeserializeCustomEvent<FullUpdateEventDTO>(eventData);
                        _simulation.LoadFrom(updateEvent.Simulation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private T DeserializeCustomEvent<T>(CustomEventData eventData)
            where T : NetworkEventDTO
        {
            var payload = eventData.Payload.ToStringUtf8();
            return JsonConvert.DeserializeObject<T>(payload, new Vector3Converter());
        }
    }
}