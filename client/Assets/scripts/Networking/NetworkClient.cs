using System;
using System.Net;
using System.Threading.Tasks;
using Networking.Protobuf.CommunicationProtocol;
using UnityEngine;

namespace Networking
{
    public class NetworkClient : BaseNetworkClient
    {
        protected override ConnectionInfo ConnectionInfo { get; } = new ConnectionInfo(IPAddress.Parse(ServerAdress), Port);

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