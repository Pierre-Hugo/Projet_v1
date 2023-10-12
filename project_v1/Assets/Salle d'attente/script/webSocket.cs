using UnityEngine;
using WebSocketSharp;

public class webSocket : MonoBehaviour
{
    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message recu de" + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };
        ws.Connect();
        ws.Send("bonjour de Unity");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        ws.Send("bye bye");
        ws.Close();
    }
}
