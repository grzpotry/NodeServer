using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public abstract class NetworkClient : INetworkClient
    {
        public abstract Task ConnectAsync();
        public abstract Task ReconnectAsync();
        public abstract void Disconnect(bool reuseSocket);
        public abstract bool Connected { get; }
        public abstract NetworkStream NetworkStream { get; }
    }
}