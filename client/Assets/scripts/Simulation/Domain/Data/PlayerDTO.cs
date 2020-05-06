using UnityEngine;

namespace Simulation.Domain.Data
{
    /// <summary>
    /// Data transfer object for <see cref="Player"/>
    /// </summary>
    public class PlayerDTO
    {
        public string Id { get; set; }
        public Vector3 Position { get; set; }
    }
}