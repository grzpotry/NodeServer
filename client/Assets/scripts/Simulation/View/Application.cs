using System;
using System.Text;
using Networking.Framework;
using Networking.Framework.Dispatchers;
using Newtonsoft.Json;
using Simulation.Domain.Network;
using Simulation.Domain.Network.Data;
using Simulation.Domain.Serialization;
using UnityEngine;
using Player = Simulation.Domain.Player;
using UserData = Networking.Framework.UserData;
using Vector3 = UnityEngine.Vector3;

namespace Simulation.View
{
    /// <summary>
    /// Example application, just for demonstration how p2p network framework can be used
    /// </summary>
    public class Application : MonoBehaviour, IUnityThreadListener
    {
        public event Action Updated;

        public string ConnectionInfo => $"Connected: {_localPeer?.IsConnected ?? false} [{PeerInfo}]";
        public bool Started { get; private set; }

        public void StartHost()
        {
            Debug.Log("Started as host");
            StartGame(isHost: true);
        }

        public void StartClient()
        {
            Debug.Log("Started as client");
            StartGame(isHost: false);
        }

        protected void Update()
        {
            Updated?.Invoke();
        }

        protected void OnDestroy()
        {
            _localPeer.Dispose();
        }

        [SerializeField]
        private SimulationView _simulationView = default;

        private ThreadSafePeer _localPeer;
        private Simulation.Domain.Simulation _simulation;
        private Player _localPlayer;
        private bool _isHost;

        private string PeerInfo => _isHost ? "Host" : "Client";

        private async void StartGame(bool isHost)
        {
            if (Started)
            {
                throw new NotSupportedException("Game is already started");
            }

            _isHost = isHost;
            Started = true;

            var tempLogin = Guid.NewGuid().ToString().Substring(0, 5);
            var userData = new UserData(tempLogin);
            var localPlayer = new Player(islocalPlayer: true, tempLogin);

            _simulation = new Simulation.Domain.Simulation(localPlayer.Id);
            _simulationView.SetSimulation(_simulation);

            _simulation.PlayerAdded += OnPlayerAdded;
            localPlayer.Moved += OnLocalPlayerMoved;

            _localPeer = new Peer(unityListener: this, userData: userData, simulation: _simulation, isHost: isHost);

            try
            {
                await _localPeer.ConnectWithMasterServerAsync();

                if (!isHost)
                {
                    await _localPeer.BroadcastEventAsync((int)CustomEventCode.FullUpdateRequest);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }

            if (_isHost)
            {
                _simulation.AddPlayer(localPlayer);
            }
        }

        private void OnPlayerAdded(Player player)
        {
            if (!player.IslocalPlayer)
            {
                return;
            }

            if (_localPlayer != null)
            {
                _localPlayer.Moved -= OnLocalPlayerMoved;
            }

            _localPlayer = player;
            _localPlayer.Moved += OnLocalPlayerMoved;
        }

        private async void OnLocalPlayerMoved(Vector3 position)
        {
            try
            {
                var moveEvent = new PlayerMovedEventDTO()
                {
                    Position = position,
                    PlayerId = _localPlayer.Id
                };

                var serializedEvent = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(moveEvent, new Vector3Converter()));
                await _localPeer.BroadcastEventAsync(serializedEvent, (int)CustomEventCode.PlayerMoved);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}