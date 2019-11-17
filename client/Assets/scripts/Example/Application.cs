using System;
using Networking.Framework;
using Networking.Framework.Dispatchers;
using Networking.Protobuf.CommunicationProtocol;
using UnityEngine;

namespace Example
{
    /// <summary>
    /// Example application
    /// </summary>
    public class Application : MonoBehaviour, IUnityThreadListener
    {
        public event Action Updated;

        protected void Start()
        {
            _localPeer = new CustomPeer(unityListener: this);

            try
            {
                _localPeer.ConnectWithHostAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //TEMP;
                _localPeer.SendRequestAsync(new HandshakePayload() {ProtocolVersion = 1},
                    OperationRequestCode.Handshake);
            }

            Updated?.Invoke();
        }

        protected void OnDestroy()
        {
            _localPeer.Dispose();
        }

        protected void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"Connected: {_localPeer.IsConnected}");
        }

        private ThreadSafePeer _localPeer;
    }
}