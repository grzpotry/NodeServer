using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking.Framework
{
    /// <summary>
    /// Client of particular network connection
    /// </summary>
    public interface INetworkClient
    {
        Task ConnectAsync();
        Task ReconnectAsync();
        void Disconnect(bool reuseSocket);
        bool Connected { get; }
        NetworkStream NetworkStream { get; }
    }
}