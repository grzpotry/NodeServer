using System;
using Networking;
using Networking.Protobuf.CommunicationProtocol;
using UnityEngine;

namespace Domain
{
    public class Application : MonoBehaviour
    {
        protected void Start()
        {
            _localPeer = new Peer();

            try
            {
                _localPeer.ConnectWithHostAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //TEMP;
                _localPeer.SendRequestAsync(new HandshakePayload() {ProtocolVersion = 1},
                    OperationRequestCode.Handshake);
            }
        }

        protected void OnDestroy()
        {
            _localPeer.Dispose();
        }

        protected void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"Connected: {_localPeer.IsConnected}");
        }

        private Peer _localPeer;
    }
}