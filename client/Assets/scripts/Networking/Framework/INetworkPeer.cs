using System.Threading.Tasks;

namespace Networking.Framework
{
    /// <summary>
    /// Peer which communicates with host
    /// </summary>
    public interface INetworkPeer
    {
        bool IsConnected { get; }

        Task ConnectWithMasterServerAsync();
    }
}