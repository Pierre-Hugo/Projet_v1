using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;

public class WebSocketController : MonoBehaviour
{
    private WebSocket ws;
    private int numberPlayerOnScene;
    private bool canJoin;
    public Text numberRoom;
    private string room;
    public List<Player> listeJoueurs;
    private bool newDataAvalid;
    private MessageEventArgs data;

    void Start()
    {
        newDataAvalid = false;
        
        listeJoueurs = new List<Player>();
        canJoin = true;

        ws = new WebSocket("ws://localhost:8080");
        ws.Connect();

        ws.OnMessage += (sender, e) =>
        {
            data = e;
            newDataAvalid = true;
        };
        

        ws.Send("bonjour de Unity");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (newDataAvalid)
        {
            //efectuer le code du OnMesssage ici
            numberRoom.text = data.Data;
            newDataAvalid=false;
        }
        if (numberPlayerOnScene != listeJoueurs.Count && canJoin)
        {
            loadNextBackground();
        }
    }

    
    public void addOnePlayer()
    {
        numberPlayerOnScene = listeJoueurs.Count;
        if (listeJoueurs.Count < 6)
        {
            Player joueurConnecte = new Player("12345", "JF");
            listeJoueurs.Add(joueurConnecte);
        }
    }

    public void removeOnePlayer(string id)
    {
        for (int i = listeJoueurs.Count - 1; i >= 0; i--)
        {
            if (listeJoueurs[i].Id == id)
            {
                listeJoueurs.RemoveAt(i);
                break;
            }
        }
    }

    private void loadNextBackground()
    {
        // Votre logique de chargement d'arrière-plan ici.
    }

    private void OnDestroy()
    {
        ws.Send("bye bye");
        ws.Close();
    }
}
