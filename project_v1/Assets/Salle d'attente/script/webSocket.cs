using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;

public class WebSocketController : MonoBehaviour
{
    private WebSocket ws;
    private int numberPlayerOnScene;
    private bool canJoin;
    public Text numberRoom;
    private string room;
    public List<Player> listeJoueurs;
    private bool newDataAvalid;
    private MessageEventArgs dataRecu;
    private string characters;
    private string id;


    void Start()
    {
        newDataAvalid = false;
        
        listeJoueurs = new List<Player>();
        canJoin = true;
        characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        ws = new WebSocket("ws://localhost:8080");
        ws.Connect();

        ws.OnMessage += (sender, e) =>
        {
            dataRecu = e;
            newDataAvalid = true;
        };

        id = GenerateRandomCode(4);
        ws.Send("UNITY" + id);

        while (!newDataAvalid) { }
        while(dataRecu.Data != "OK")
        {
            if(dataRecu.Data == "ID already in use") 
            {
                id = GenerateRandomCode(4);
                ws.Send("UNITY" + id);
            }
        }
        numberRoom.text = id;

    }

    // Update is called once per frame
    void Update()
    {
        if (newDataAvalid)
        {
            
            
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
    public string GenerateRandomCode(int length)
    {
        string code = "";
        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, characters.Length);
            code += characters[randomIndex];
        }
        return code;
    }


    private void OnDestroy()
    {
        ws.Send("bye bye");
        ws.Close();
    }
}
