using System;

namespace Domain
{
    public interface IClient: IDisposable
    {
        event Action<IClient> Connected;
        event Action<IClient> Disconnected;

        bool IsConnected { get; }

        void ConnectWithHost(string adress, int port);
        void Update();
    }
}
