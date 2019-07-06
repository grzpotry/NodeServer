using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ClientConnection : MonoBehaviour
{
    private Socket _tempSocket;

    // Start is called before the first frame update
    void Start()
    {
        var hostAdress = "localhost";
        int port = 3000;

        _tempSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        _tempSocket.Connect(hostAdress, port);



        Debug.Log($"Connected with {hostAdress} {port}");
    }

    void Update()
    {
        _tempSocket.Send(Encoding.UTF8.GetBytes("packet"));
        Debug.Log("IsConnected: " + _tempSocket.Connected);
    }

    private void OnDisable()
    {
        _tempSocket.Disconnect(false);
        _tempSocket.Dispose();
    }
}
