namespace Simulation.Domain.Network.Data
{
    /// <summary>
    /// Custom application-specific events which are sent between connected players
    /// </summary>
    public enum CustomEventCode
    {
        PlayerMoved,
        FullUpdateRequest,
        FullUpdate,
    }
}