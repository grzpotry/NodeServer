using System.Net;
using System.Threading.Tasks;
using Networking.Framework;
using Networking.Protobuf.CommunicationProtocol;

namespace Networking
{
    /// <summary>
    /// Peer for particular application. TODO: move to separate assembly with example application
    /// </summary>
    public class Peer : BasePeer
    {
        protected override ConnectionInfo ConnectionInfo { get; } =
            new ConnectionInfo(IPAddress.Parse(ServerAdress), Port);

        protected override async Task HandleResponseAsync(OperationResponse response)
        {
            // Debug.Log("H")
        }

        protected override async Task HandleEventAsync(EventData eventData)
        {
            //
        }

        private const string ServerAdress = "127.0.0.1";
        private const int Port = 3000;
    }
}