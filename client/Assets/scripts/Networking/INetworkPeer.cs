using System.Threading.Tasks;
using Google.Protobuf;
using Networking.Protobuf.CommunicationProtocol;

namespace Networking
{
    /// <summary>
    ///
    /// </summary>
    public interface INetworkPeer
    {
        bool IsConnected { get; }

        Task ConnectWithHostAsync();

        Task SendRequestAsync(IMessage message, OperationRequestCode code);
    }
}