using System;
using Simulation.Domain.Data;
using UnityEngine;

namespace Simulation.Domain
{
    /// <summary>
    /// Representation of player which lives within <see cref="Simulation"/>
    /// </summary>
    public class Player
    {
        public event Action<Vector3> Moved;

        public Vector3 Position => _position;
        public bool IslocalPlayer { get; }
        public string Id { get; private set; }

        public Player(bool islocalPlayer, string id)
        {
            Id = id;
            IslocalPlayer = islocalPlayer;
        }

        public Player(bool isLocalPlayer, PlayerDTO dto)
        {
            IslocalPlayer = isLocalPlayer;
            LoadFrom(dto);
        }

        public void LoadFrom(PlayerDTO dto)
        {
            Id = dto.Id;
            _position = dto.Position;
        }

        public PlayerDTO Save()
        {
            return new PlayerDTO()
            {
                Id = Id,
                Position = Position
            };
        }

        public void MoveTo(Vector3 position)
        {
            _position = position;
            Moved?.Invoke(Position);
        }

        private Vector3 _position;
    }
}