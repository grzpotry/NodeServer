using System;
using System.Collections.Generic;
using Simulation.Domain;
using Simulation.Domain.Controller;
using UnityEngine;

namespace Simulation.View
{
    /// <summary>
    /// Manages all monos in <see cref="Simulation"/>
    /// </summary>
    public class SimulationView : MonoBehaviour, IDisposable
    {
        public void Dispose()
        {
            if (_simulation != null)
            {
                _simulation.PlayerAdded -= OnPlayerAdded;
                _simulation.PlayerRemoved -= OnPlayerRemoved;
            }

            _simulation = null;
        }

        public void SetSimulation(Simulation.Domain.Simulation simulation)
        {
            _simulation = simulation ?? throw new ArgumentNullException(nameof(simulation));
            _simulation.PlayerAdded += OnPlayerAdded;
            _simulation.PlayerRemoved += OnPlayerRemoved;
        }

        [SerializeField]
        private PlayerPhysics _playerPrefab = default;

        [SerializeField]
        private Material _localPlayerMat = default;

        [SerializeField]
        private Material _remotePlayerMat = default;

        private readonly Dictionary<Player, PlayerPhysics> _players = new Dictionary<Player, PlayerPhysics>();
        private Simulation.Domain.Simulation _simulation;

        private void OnPlayerRemoved(Player player)
        {
            if (!_players.TryGetValue(player, out var playerPhysics))
            {
                return;
            }

            _players.Remove(player);
            Destroy(playerPhysics.gameObject);
            playerPhysics.Dispose();
        }

        private void OnPlayerAdded(Player player)
        {
            var playerPhysics = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            playerPhysics.SetPlayer(player);

            playerPhysics.gameObject.GetComponent<MeshRenderer>().material = player.IslocalPlayer ? _localPlayerMat : _remotePlayerMat;

            if (player.IslocalPlayer)
            {
                var controller = new PlayerController(player);
                playerPhysics.SetController(controller);
            }

            _players.Add(player, playerPhysics);
            Debug.Log("Added player " + player.Id);
        }
    }
}