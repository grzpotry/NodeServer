using System.Threading.Tasks;
using Google.Protobuf;
using Networking.Protobuf.CommunicationProtocol;

namespace Networking.Framework
{
    /// <summary>
    /// Peer which communicates with host
    /// </summary>
    public interface INetworkPeer
    {
        bool IsConnected { get; }

        Task ConnectWithHostAsync();

        Task SendRequestAsync(IMessage message, OperationRequestCode code);
    }
}