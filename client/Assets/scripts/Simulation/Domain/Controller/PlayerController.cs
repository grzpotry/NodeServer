using System;
using UnityEngine;

namespace Simulation.Domain.Controller
{
    /// <summary>
    /// Controls <see cref="Player"/>'s movement
    /// </summary>
    public class PlayerController
    {
        public Player Player { get; }

        public PlayerController(Player player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public void Update()
        {
            var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (dir.sqrMagnitude > 0.1f)
            {
                var offset = dir * (Time.deltaTime * _movementSpeed);
                Player.MoveTo(Player.Position + offset);
            }
        }

        private float _movementSpeed = 5;
    }
}