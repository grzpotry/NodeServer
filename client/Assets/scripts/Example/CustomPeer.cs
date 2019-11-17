using System.Net;
using Networking.Framework;
using Networking.Framework.Dispatchers;
using Networking.Protobuf.CommunicationProtocol;

namespace Example
{
    /// <summary>
    /// Custom peer
    /// </summary>
    public class CustomPeer : ThreadSafePeer
    {
        public CustomPeer(IUnityThreadListener unityListener)
            : base(unityListener)
        {
        }

        protected override ConnectionInfo ConnectionInfo { get; } =
            new ConnectionInfo(IPAddress.Parse(ServerAdress), Port);

        protected override void HandleResponse(OperationResponse response)
        {
            //throw new System.NotImplementedException();
        }

        protected override void HandleEvent(EventData eventData)
        {
            //throw new System.NotImplementedException();
        }

        private const string ServerAdress = "127.0.0.1";
        private const int Port = 3000;
    }
}