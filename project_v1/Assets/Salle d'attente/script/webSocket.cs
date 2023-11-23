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
    private bool idConfirmer;


    void Start()
    {
        listScript = FindObjectOfType<Liste>();
        newDataAvalid = false;
        nbMaxJoueurs = 6;
        listeJoueurs = new List<Player>();
        canJoin = true;
        idConfirmer = false;
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

        
        while (!idConfirmer)
        {
            while (!newDataAvalid)
            {
                if (dataRecu.Data == "ID already in use")
                {
                    id = GenerateRandomCode(4);
                    ws.Send("UNITY" + id);
                }
                else if(dataRecu.Data == "OK")
                {
                    numberRoom.text = id;
                    idConfirmer = true;
                }
                newDataAvalid = false;

            }
        }
        
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
            AddPlayerToScene();
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
            // enovyer un message pour dire que la salle est pleine

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

    private void AddPlayerToScene()
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
