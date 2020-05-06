using Newtonsoft.Json;

namespace Simulation.Domain.Network.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class NetworkEventDTO
    {
    }
}