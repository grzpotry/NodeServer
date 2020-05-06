using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Networking.Framework
{
    /// <summary>
    /// Client of particular network connection
    /// </summary>
    public interface INetworkClient
    {
        bool Connected { get; }
        NetworkStream NetworkStream { get; }

        Task ConnectAsync();

        Task ConnectAsync(CancellationTokenSource cancelTokenSource);

        Task ReconnectAsync();

        Task ReconnectAsync(CancellationTokenSource cancelTokenSource);

        void Disconnect(bool reuseSocket);
    }
}