using Simulation.Domain.Data;

namespace Simulation.Domain.Network.Data
{
    public class FullUpdateEventDTO : NetworkEventDTO
    {
        public SimulationDTO Simulation { get; set; }
    }
}