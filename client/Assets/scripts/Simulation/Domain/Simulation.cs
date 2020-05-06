using System;
using System.Collections.Generic;
using System.Linq;
using Networking.Framework.Utils;
using Simulation.Domain.Data;
using UnityEngine;

namespace Simulation.Domain
{
    /// <summary>
    /// Handles core simulation logic
    /// </summary>
    public class Simulation
    {
        public event Action<Player> PlayerAdded;
        public event Action<Player> PlayerRemoved;

        public IReadOnlyList<Player> Players => _players;

        public Simulation(string localPlayerId)
        {
            _localPlayerId = localPlayerId;
        }

        public void LoadFrom(SimulationDTO dto)
        {
            _players.ReplaceWith(dto.Players, _ =>
            {
                var player = new Player(isLocalPlayer: _localPlayerId == _.Id, _);
                PlayerAdded?.Invoke(player);
                return player;
            }, onRemoved: _ => PlayerRemoved?.Invoke(_));
        }

        public SimulationDTO Save()
        {
            return new SimulationDTO()
            {
                Players = _players.Select(_ => _.Save()).ToList()
            };
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
            PlayerAdded?.Invoke(player);
        }

        public void RemovePlayer(string id)
        {
            var player = _players.FirstOrDefault(_ => _.Id == id);

            if (player == null)
            {
                Debug.LogError($"Player {id} does not exists in simulation.");
                return;
            }

            RemovePlayer(player);
        }

        private readonly List<Player> _players = new List<Player>();
        private readonly string _localPlayerId;

        private void RemovePlayer(Player player)
        {
            _players.Remove(player);
            PlayerRemoved?.Invoke(player);
        }
    }
}