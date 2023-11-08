using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
            data = e;
            newDataAvalid = true;
        };




        StartCoroutine(SendAndCheckId());


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

    IEnumerator SendAndCheckId()
    {
        id = GenerateRandomCode(4);
        ws.Send("UNITY" + id);

        while (!newDataAvalid)
        {
            yield return null; // Attendez que la réponse du serveur soit disponible
        }

        // Une fois que la réponse du serveur est reçue, traitez-la
        string response = data.Data;

        if (response == "ID valide")
        {
            numberRoom.text = id; // Affichez l'ID dans l'UI
            // Continuez avec le reste de votre logique ici
        }
        else
        {
            // L'ID n'est pas valide, régénérez un nouvel ID
            StartCoroutine(SendAndCheckId());
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
