using System;
using Simulation.Domain;
using Simulation.Domain.Controller;
using UnityEngine;

namespace Simulation.View
{
    /// <summary>
    /// <see cref="Player"/>'s representation in actual <see cref="SimulationView"/>
    /// </summary>
    public class PlayerPhysics : MonoBehaviour
    {
        public void SetPlayer(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void Dispose()
        {
            _player = null;
        }

        public void SetController(PlayerController controller)
        {
            _controller = controller;

            if (_controller.Player != _player)
            {
                throw new Exception("Mismatched player and controller");
            }
        }

        protected void Awake()
        {
            _transform = transform;
        }

        protected void Update()
        {
            if (_player == null)
            {
                return;
            }

            _controller?.Update();
            _transform.position = _player.Position; //TODO: replace with automatic binding
        }

        private Player _player;
        private PlayerController _controller;
        private Transform _transform;
    }
}