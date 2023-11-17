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
    private int nbMaxJoueurs;
    private Liste listScript;


    void Start()
    {
        listScript = FindObjectOfType<Liste>();
        newDataAvalid = false;
        nbMaxJoueurs = 6;
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
        addOnePlayer("12345","jf",Color.red);
        addOnePlayer("67890", "peach", Color.blue);
        removeOnePlayer("12345");
        listScript.AjouterListe("JF",Color.green );
        listScript.AjouterListe("CIMENT", Color.cyan);
        listScript.AjouterListe("CIMENT", Color.cyan);
        listScript.AjouterListe("CIMENT", Color.cyan);
        listScript.AjouterListe("CIMENT", Color.cyan);
        listScript.AjouterListe("CIMENT", Color.cyan);

        //listScript.retirerListe("PEACH");

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


    public void addOnePlayer(string ID, string PSEUDO, Color COULEUR)
    {
        numberPlayerOnScene = listeJoueurs.Count;
        if (listeJoueurs.Count < nbMaxJoueurs)
        {
            Player joueurConnecte = new Player(ID, PSEUDO, COULEUR);
            listeJoueurs.Add(joueurConnecte);
        }
        else
        {
            ws.Send("Coucou tu veut voir ma bite");
        }
    }

    public void removeOnePlayer(string id)
    {
        foreach (Player joueur in listeJoueurs)
        {
            if (joueur.Id == id)
            {
                listeJoueurs.Remove(joueur);
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
