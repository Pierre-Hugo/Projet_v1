using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;
//using static Player;


public class webSocket : MonoBehaviour
{
    private WebSocket ws;
    public GameObject Player;
    private int numberPlayerOnScene;
    private bool canJoin;
    public Text numberRoom;
    private string room;

    void Start()
    {
        canJoin = true;
  
        ws = new WebSocket("ws://localhost:8080");
        

        
        ws.Connect();
        
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message recu de" + ((WebSocket)sender).Url + ", Data : " + e.Data);
            
            room = e.Data;
            
        };
        addOnePlayer();
        ws.Send("bonjour de Unity");
    }

    // Update is called once per frame
    void Update()
    {
        if (room != numberRoom.text) numberRoom.text = room; //s'arranger pour que ca s'update seulement quand je recois l'id   
        
        //  numberPlayerOnScene = listeJoueurs.Count;
        //if(numberPlayerOnScene != listeJoueurs.Count&&canJoin)
        //{
        loadNextBackground();
        //}

    }

    public void addOnePlayer()
    {
        //if (listeJoueurs.Count < 6)
        //{
            // ne peut pas utiliser new Player, doit être changé
        //    Player joueurConnecte = new Player("12345", "Funz");
          //  listeJoueurs.Add(joueurConnecte);
      //  }
    }

    public void removeOnePlayer(string id)
    {
        //foreach(Player joueur in listeJoueurs)
       // {
         //   if(joueur.getId() == id)
           // {
             //   listeJoueurs.Remove(joueur);
            //}
        //}
    }

    private void loadNextBackground()
    {

    }

    private void OnDestroy()
    {
        ws.Send("bye bye");
        ws.Close();
    }
}
