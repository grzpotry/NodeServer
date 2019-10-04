using System;
using UnityEngine;

namespace Domain
{
    public class Application : MonoBehaviour
    {
        protected void Start()
        {
            _localClient = new Client();

            try
            {
                _localClient.ConnectWithHostAsync(ServerAdress, Port);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Update()
        {
            _localClient.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _localClient.SendMessageAsync("Hello server !").ContinueWith(_ => Debug.Log("Message sent"));
            }
        }

        protected void OnDestroy()
        {
            _localClient.Dispose();
        }

        protected void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 50), $"Connected: {_localClient.IsConnected}");
        }

        private IClient _localClient;
        private const string ServerAdress = "127.0.0.1";
        private const int Port = 3000;
    }
}