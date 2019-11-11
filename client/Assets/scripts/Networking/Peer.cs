using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Networking.Framework;
using Networking.Protobuf.CommunicationProtocol;
using UnityEngine;

namespace Networking
{
    /// <summary>
    /// Peer for particular application. TODO: move to separate assembly with example application
    /// </summary>
    public class Peer : BasePeer
    {
        public Peer()
        {
            _unityThread = Thread.CurrentThread.ManagedThreadId;
        }

        protected override ConnectionInfo ConnectionInfo { get; } =
            new ConnectionInfo(IPAddress.Parse(ServerAdress), Port);

        protected override Task HandleResponseAsync(OperationResponse response)
        {
            if (Thread.CurrentThread.ManagedThreadId != _unityThread)
            {
                Debug.LogError("TODO: [IMPLEMENT] : Handled response in non-unity thread.");
            }

            return Task.CompletedTask;
        }

        protected override Task HandleEventAsync(EventData eventData)
        {
            return Task.CompletedTask;
        }

        private const string ServerAdress = "127.0.0.1";
        private const int Port = 3000;
        private readonly int _unityThread;
    }
}