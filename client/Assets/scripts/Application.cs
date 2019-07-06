using UnityEngine;

namespace Domain
{
    public class Application : MonoBehaviour
    {
        protected void Start()
        {
            _localClient = new Client(autoReconnect: true);
            _localClient.ConnectWithHost(ServerAdress, Port);
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

        private IClient _localClient;
        private const string ServerAdress = "localhost";
        private const int Port = 3000;
    }
}