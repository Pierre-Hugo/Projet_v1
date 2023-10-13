using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;


public class webSocket : MonoBehaviour
{
    public class Player
    {
        public string pseudo;
        public int points;
        public string couleur;
        public string id;

        public Player(string _id, string _pseudo)
        {
            pseudo = _pseudo;
            id = _id;
        }
        
        public void setId(string _id) 
        {
            id = _id;
        }

        public string getId()
        {
            return id;
        }
        public void setCouleur(string _couleur)
        {
            couleur = _couleur;
        }

        public string getCouleur()
        {
            return couleur;
        }
        public void setPoints(int _points)
        {
            points = _points;
        }
        public int getPoints()
        {
            return points;
        }

        public string getPseudo()
        {
            return pseudo;
        }
        public void setPseudo(string _pseudo)
        {
            pseudo = _pseudo;
        }

        public void addPoints(int _points)
        {
            points += _points;
        }




    }

    private WebSocket ws;
   
    private int numberPlayerOnScene;
    private bool canJoin;
    public Text numberRoom;
    private string room;
    public List<Player> listeJoueurs;

    void Start()
    {
        listeJoueurs= new List<Player> ();
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
        
        
        if(numberPlayerOnScene != listeJoueurs.Count && canJoin)
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
        numberPlayerOnScene = listeJoueurs.Count;
        foreach (Player joueur in listeJoueurs)
        {
            if(joueur.getId() == id)
            {
                listeJoueurs.Remove(joueur);
            }
        }
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
