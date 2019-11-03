using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Networking.Protobuf.CommunicationProtocol;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public interface INetworkClient
    {
        event Action<INetworkClient> Connected;
        event Action<INetworkClient> Disconnected;

        bool IsConnected { get; }

        Task ConnectWithHostAsync();
        void Update();

        Task SendRequestAsync(IMessage message, OperationRequestCode code);
    }
}