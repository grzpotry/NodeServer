using System;
using System.Threading.Tasks;

namespace Domain
{
    public interface INetworkClient: IDisposable
    {
        event Action<INetworkClient> Connected;
        event Action<INetworkClient> Disconnected;

        bool IsConnected { get; }

        Task ConnectWithHostAsync(string adress, int port);
        void Update();
    }
}
