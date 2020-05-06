using System.Collections.Generic;

namespace Simulation.Domain.Data
{
    /// <summary>
    /// Data transfer object for <see cref="Simulation"/>
    /// </summary>
    public class SimulationDTO
    {
        public List<PlayerDTO> Players = new List<PlayerDTO>();
    }
}