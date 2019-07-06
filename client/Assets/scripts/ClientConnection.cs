using System;
using UnityEngine;
using WebSocketSharp;

public class ClientConnection : MonoBehaviour
{
    // Start is called before the first frame update
    protected void Start()
    {
        _socket = new WebSocket("ws://localhost:3000/socket.io/?EIO=2&transport=websocket");
        _socket.OnOpen += SocketOnOnOpen;

        _socket.Connect();
        _socket.Log.Output = (data, s) => { Debug.Log(s);};
    }

    protected void Update()
    {
        Debug.Log("IsConnected: " + _socket.IsAlive);
    }

    private WebSocket _socket;

    private void SocketOnOnOpen(object sender, EventArgs e)
    {
        Debug.Log("connected");
    }
    private void OnDisable()
    {
        _socket.Close();
    }
}
