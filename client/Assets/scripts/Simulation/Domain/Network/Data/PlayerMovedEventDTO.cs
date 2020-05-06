using UnityEngine;

namespace Simulation.Domain.Network.Data
{
    public class PlayerMovedEventDTO : NetworkEventDTO
    {
        public string PlayerId { get; set; }
        public Vector3 Position { get; set; }
    }
}