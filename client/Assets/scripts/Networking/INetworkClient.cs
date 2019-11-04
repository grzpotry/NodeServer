using System.Threading.Tasks;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public interface INetworkClient
    {
        Task ConnectAsync();
        Task ReconnectAsync();
        void Disconnect(bool reuseSocket);
        bool Connected { get; }
    }
}