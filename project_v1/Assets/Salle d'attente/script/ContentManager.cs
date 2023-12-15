using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;

public class ContentManager : MonoBehaviour
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
    public GameObject canvaError;
    public Button boutonRetour;


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

        if (ws.IsAlive)
        {
            id = GenerateRandomCode(4);
            ws.Send("UNITY" + id);
            

            while (!idConfirmer)
            {
                if (newDataAvalid)
                {
                    if (dataRecu.Data == "ID already in use")
                    {
                        id = GenerateRandomCode(4);
                        ws.Send("UNITY" + id);
                    }
                    else if (dataRecu.Data == "OK")
                    {
                        numberRoom.text = id;
                        idConfirmer = true;
                    }
                    newDataAvalid = false;
                }
               
            }
        }
        else
        {
            //afficher message d'erreur impossible de se connecter
            canvaError.SetActive(true);
            boutonRetour.GetComponent<MonoBehaviour>().enabled = false;
            boutonRetour.interactable = false;

        }

        addOnePlayer("12345", "jf", Color.red);
        addOnePlayer("67890", "peach", Color.cyan);
        listScript.AjouterListe("JF", Color.red);
        listScript.AjouterListe("PEACH", Color.cyan);

        //listScript.retirerListe("PEACH");

    }

    // Update is called once per frame
    void Update()
    {
        if (newDataAvalid)
        {
            string[] messageComplet = dataRecu.Data.Split(":");
            string[] messageRecu = messageComplet[1].Split(",");

            string instruction = messageRecu[0];

            //exemple de message recu
            //string[] messageRecu = "CLIENTABCD:NP,Xx_coolGuy_xX,BLUE"

            //NP pour New Player
            if (instruction == "NP")
            {
                if (listeJoueurs.Count < nbMaxJoueurs)
                {
                    bool donneValide = true;
                    string idRecu = messageComplet[0];
                    string pseudoRecu = messageRecu[1];
                    Color couleurRecu = conversionStringColor(messageRecu[2]);

                    foreach (Player joueur in listeJoueurs)
                    {
                        if (joueur.Id == idRecu || joueur.Pseudo == pseudoRecu || joueur.Couleur == couleurRecu)
                        {
                            ws.Send(idRecu + ":Donne invalides");
                            donneValide = false;
                            
                            break;
                        }
                        
                    }
                    if (donneValide)
                    {
                        addOnePlayer(idRecu, pseudoRecu, couleurRecu);
                        listScript.AjouterListe(pseudoRecu, couleurRecu);
                    }
                }
                else
                {
                    ws.Send(messageRecu[1] + ": Salle Pleine");
                }
            }
            if(instruction == "DC")
            {
                foreach(Player joueur in listeJoueurs)
                {
                    if(joueur.Id == messageComplet[0]) // chercher l'id dans la liste qui correspond à celui recu
                    {
                        removeOnePlayer(joueur.Id);
                        listScript.retirerListe(joueur.Pseudo);
                        break;
                    }
                }
                
                
            }




            newDataAvalid = false;
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

    private Color conversionStringColor(string colorRecu)
    {
        Color color = Color.white;
        switch (colorRecu)
        {
            case "BLUE":
                color = Color.blue;
                break;
            case "RED":
                color = Color.red;
                break;
            case "WHITE":
                color = Color.white;
                break;
            case "BLACK":
                color = Color.black;
                break;
            case "CYAN":
                color = Color.cyan;
                break;
            case "GRAY":
                color = Color.gray;
                break;
            case "GREEN":
                color = Color.green;
                break;
            case "MAGENTA":
                color = Color.magenta;
                break;
            case "YELLOW":
                color = Color.yellow;
                break;

        }

        return color;
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

    public void showBackBouton() 
    {
        boutonRetour.GetComponent<MonoBehaviour>().enabled = true;
        boutonRetour.interactable = true;
    }

    private void OnDestroy()
    {
        if (ws.IsAlive)
        {
            ws.Send("bye bye");
            ws.Close();
        }
    }

    
}
