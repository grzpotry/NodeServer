using System;
using Networking;
using UnityEngine;

namespace Domain
{
    public class Application : MonoBehaviour
    {
        protected void Start()
        {
            _localClient = new NetworkClient();

            try
            {
                _localClient.ConnectWithHostAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Update()
        {
            _localClient.Update();
        }

        protected void OnDestroy()
        {
            _localClient.Dispose();
        }

        protected void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"Connected: {_localClient.IsConnected}");
        }

        private NetworkClient _localClient;
    }
}