using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;



public class ContentManager : MonoBehaviour
{
    private WebSocket ws;
    private int numberPlayerOnScene;
    public Text numberRoom;
    private string room;
    public List<Player> listeJoueurs;
    private List<MessageEventArgs> listeDataRecu;
    private string characters;
    private string id;
    private int nbMaxJoueurs;
    private Liste listScript;
    private bool idConfirmer;
    private object lockObject;
    public GameObject canvaError;
    public Button boutonRetour;
    public Button boutonPlay;


    void Start()
    {
        listScript = FindObjectOfType<Liste>();
        nbMaxJoueurs = 6;
        listeJoueurs = new List<Player>();
        listeDataRecu = new List<MessageEventArgs>();
        idConfirmer = false;
        lockObject = new object();
        characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        boutonPlay.interactable = false;


        ws = new WebSocket("ws://localhost:8080");


        ws.Connect();



        ws.OnMessage += (sender, e) =>
        {
            
            lock (lockObject)
            {
                listeDataRecu.Add(e);
                
            }
        };

        

        if (ws.IsAlive)
        {
            id = GenerateRandomCode(4);
            ws.Send("UNITY" + id);
            

            while (!idConfirmer)
            {
                if (listeDataRecu.Count > 0)
                {
                    lock (lockObject) {
                        List<MessageEventArgs> listeDonne = new List<MessageEventArgs>(listeDataRecu);

                        foreach (MessageEventArgs data in listeDonne)
                        {
                            if (data.Data == "ID already in use")
                            {
                                id = GenerateRandomCode(4);
                                ws.Send("UNITY" + id);
                            }
                            else if (data.Data == "OK")
                            {
                                numberRoom.text = id;
                                idConfirmer = true;
                            }
                            listeDataRecu.Remove(data);
                        }
                    }
                }
               
               
            }
        }
        else
        {
            //afficher message d'erreur impossible de se connecter
            canvaError.SetActive(true);
            boutonRetour.gameObject.SetActive(false);


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
        if (listeDataRecu.Count>0)
        {
            lock (lockObject)
            {
                List<MessageEventArgs> listeDonne = new List<MessageEventArgs>(listeDataRecu); ;

                foreach (MessageEventArgs dataRecu in listeDonne)
                {


                    string[] messageComplet = dataRecu.Data.Split(":");
                    string idRecu = messageComplet[0];
                    string[] messageRecu = messageComplet[1].Split(",");

                    string instruction = messageRecu[0];


                    //NP pour New Player
                    //exemple de message recu
                    //string[] messageRecu = "NP,Xx_coolGuy_xX,BLUE"
                    if (instruction == "NP")
                    {
                        if (listeJoueurs.Count < nbMaxJoueurs)
                        {
                            bool donneValide = true;
                            
                            string pseudoRecu = messageRecu[1];
                            Color couleurRecu = conversionStringColor(messageRecu[2]);

                            foreach (Player joueur in listeJoueurs)
                            {
                                //vérifie si l'id, le pseudo ou la couleur est déjà utilisé
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
                                if(listeJoueurs.Count > 2) 
                                {
                                boutonPlay.interactable = true;
                                }
                                
                            }
                        }
                        else
                        {
                            ws.Send(messageRecu[1] + ": Salle Pleine");
                        }
                    }

                    //DC pour Disconnected
                    //exemple de message recu
                    //string[] messageRecu = "DC"
                    if (instruction == "DC")
                    {
                        foreach (Player joueur in listeJoueurs)
                        {
                            if (joueur.Id == idRecu) // chercher l'id dans la liste qui correspond à celui recu
                            {
                                removeOnePlayer(joueur.Id);
                                listScript.retirerListe(joueur.Pseudo);
                                if (listeJoueurs.Count <= 2)
                                {
                                    boutonPlay.interactable = false;
                                }
                                break;
                            }
                        }


                    }
                    //CC pour Change Color
                    //exemple de message recu
                    //string[] messageRecu = "CC,BLUE"
                    if (instruction == "CC")
                    {
                        Color couleur = conversionStringColor(messageRecu[1]);
                        string Pseudo = GetNameById(idRecu);
                        bool couleurAlreadyUse = false;

                        foreach (Player joueur in listeJoueurs)
                        {
                            if(joueur.Couleur == couleur)
                            {
                                couleurAlreadyUse = true;
                            }
                        }

                        if (Pseudo != null && !couleurAlreadyUse) 
                        {
                           listScript.ChangerCouleur(Pseudo, couleur);
                            foreach (Player joueur in listeJoueurs)
                            {
                                if (joueur.Pseudo == Pseudo)
                                {
                                    joueur.Couleur = couleur;
                                }
                            }
                        }
                    }

                    listeDataRecu.Remove(dataRecu);
                }
            }
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

    private string GetNameById(string id)
    {
        foreach (Player joueur in listeJoueurs)
        {
            if (joueur.Id == id)
            {
                return joueur.Pseudo;
            }
        }
        return null;
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
        boutonRetour.gameObject.SetActive(true);
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
