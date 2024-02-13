using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.Animations;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

public class ContentManager : MonoBehaviour
{
    private WebSocket ws;
    public Text numberRoom;
    private List<Player> listeJoueurs;
    private List<MessageEventArgs> listeDataRecu;
    private string characters;
    private string id;
    private int nbMaxJoueurs;
    private Liste listScript;
    private bool idConfirmer;
    private object lockObject;
    public GameObject canvaError;
    public Button boutonRetour;
    public Button boutonStart;
    public GameObject background;
    private bool isGamePlaying;
    private bool isPlayerAnswering;
    private bool isPlayerVoting;




    void Start()
    {
        listScript = FindObjectOfType<Liste>();
        nbMaxJoueurs = 6;
        listeJoueurs = new List<Player>();
        listeDataRecu = new List<MessageEventArgs>();
        idConfirmer = false;
        lockObject = new object();
        characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        boutonStart.interactable = true;
        isGamePlaying = false;
        isPlayerAnswering = false;
        isPlayerVoting = false;







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
            id = "28EP";
            ws.Send("UNITY" + id);


            while (!idConfirmer)
            {
                if (listeDataRecu.Count > 0)
                {
                    lock (lockObject)
                    {
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

        addOnePlayerWord("USER1234", "jf", Color.red, "Salut");
        addOnePlayerWord("USERABCD", "peach", Color.cyan, "Coucou");
        addOnePlayerWord("USER5678", "Simone", Color.blue, "Allo");
        addOnePlayerWord("USEREFGH", "Flex", Color.green, "Bonjour");
        listScript.AjouterListe("Flex", Color.green);
        listScript.AjouterListe("Simone", Color.blue);
        listScript.AjouterListe("JF", Color.red);
        listScript.AjouterListe("PEACH", Color.cyan);

    }

    // Update is called once per frame
    void Update()
    {
        if (listeDataRecu.Count > 0)
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
                    //string[] messageRecu = "NP,Xx_coolGuy_xX,BLUE,[...]"
                    if (instruction == "NP")
                    {
                        if (!isGamePlaying)
                        {
                            if (listeJoueurs.Count < nbMaxJoueurs && messageRecu.Length > 4)
                            {
                                bool donneValide = true;

                                string pseudoRecu = messageRecu[1];
                                Color couleurRecu = conversionStringColor(messageRecu[2]);

                                foreach (Player joueur in listeJoueurs)
                                {
                                    //v�rifie si l'id, le pseudo ou la couleur est d�j� utilis�
                                    if (joueur.Id == idRecu || joueur.Pseudo == pseudoRecu || joueur.Couleur == couleurRecu)
                                    {
                                        ws.Send(idRecu + ":Donne invalides");
                                        donneValide = false;

                                        break;
                                    }

                                }
                                if (donneValide)
                                {

                                    string typePlayer = messageRecu[3];
                                    switch (typePlayer)//ajoute le bon type de Player
                                    {
                                        case "PIC": // exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,PIC,(code de l'image), TRUE"
                                            if (messageRecu.Length > 5)
                                            {
                                                break;
                                            }
                                            string img = messageRecu[4];
                                            bool isDraw = messageRecu[5] == "TRUE";
                                            addOnePlayerPicture(idRecu, pseudoRecu, couleurRecu, img, isDraw);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;

                                        case "VID": //exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,VID,(code de la video)"
                                            string vid = messageRecu[4];
                                            addOnePlayerVideo(idRecu, pseudoRecu, couleurRecu, vid);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;

                                        case "WRD": //exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,WRD,Coucou"
                                            string word = messageRecu[4];
                                            addOnePlayerWord(idRecu, pseudoRecu, couleurRecu, word);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;
                                    }



                                    //  test.GetComponent < Image >().sprite = Sprite.Create(joueur.imageTexture, new Rect(0, 0, joueur.imageTexture.width, joueur.imageTexture.height), Vector2.zero);

                                    if (listeJoueurs.Count > 2)
                                    {
                                        boutonStart.interactable = true;
                                    }
                                }
                            }
                            else
                            {
                                ws.Send(idRecu + ":Salle Pleine");
                            }
                        }
                        else
                        {
                            //Si la partie est d�j� commencer, je vais mettre le joueur comme connecter
                            Player joueur = GetPlayerById(idRecu);
                            if (joueur != null)
                            {
                                joueur.PlayerConnected(true);
                            }
                            else
                            {
                                ws.Send(idRecu + ":Game already started");
                            }

                        }
                    }

                    //DC pour Disconnected
                    //exemple de message recu
                    //string[] messageRecu = "DC"
                    else if (instruction == "DC" && !isGamePlaying)
                    {
                        Player joueur = GetPlayerById(idRecu);
                        if (joueur != null)
                        {
                            removeOnePlayer(joueur.Id);
                            listScript.retirerListe(joueur.Pseudo);
                            if (listeJoueurs.Count <= 2)
                            {
                                boutonStart.interactable = false;
                            }
                        }

                    }
                    //CC pour Change Color
                    //exemple de message recu
                    //string[] messageRecu = "CC,BLUE"
                    else if (instruction == "CC" && !isGamePlaying)
                    {
                        Color couleur = conversionStringColor(messageRecu[1]);
                        string Pseudo = GetNameById(idRecu);
                        bool couleurAlreadyUse = false;

                        foreach (Player joueur in listeJoueurs)
                        {
                            if (joueur.Couleur == couleur)
                            {
                                couleurAlreadyUse = true;
                                break;
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
                                    break;
                                }
                            }
                        }
                    }

                    //AN pour Change Answer
                    //exemple de message recu
                    //string[] messageRecu = "AN,Chien"
                    else if (instruction == "AN" && isGamePlaying && isPlayerAnswering)
                    {
                        string reponse = messageRecu[1];
                        Player joueur = GetPlayerById(idRecu);
                        if (joueur != null)
                        {
                            joueur.answer = reponse;
                        }
                        else
                        {
                            ws.Send(idRecu + ":user unknown");
                        }

                    }

                    //VO pour Vote
                    //exemple de message recu
                    //string[] messageRecu = "VO,USERABCD"
                    else if (instruction == "VO" && isGamePlaying && isPlayerVoting)
                    {
                        string vote = messageRecu[1];
                        Player joueur = GetPlayerById(idRecu);
                        if (joueur != null)
                        {
                            joueur.vote = vote;
                        }
                    }

                    //message envoyer par le serveur
                    //exemple de message recu
                    //string[] messageRecu = "Client ID not found"
                    else if (instruction == "Client ID not found")
                    {
                        //ce message va etre recu si on envoye un message � un id qui est introuvable, donc quelqu'un de d�connect�, mais qui l'a d�j� �t�
                        //Je met donc son �tat isConnected � false pour le moment et il peux se reconnecter plus tard
                        //normalement, cela ne peux arriver que quand la partie est commenc�
                        Player joueur = GetPlayerById(idRecu);
                        if (joueur != null)
                        {
                            joueur.PlayerConnected(false);
                        }

                    }

                    else
                    {
                        ws.Send(idRecu + ":Impossibe de traiter la demande");
                    }

                    listeDataRecu.Remove(dataRecu);
                }
            }
        }


    }


    void LoadSpriteFromDataUrl(string dataUrl, Image image)
    {
        // Divisez l'URL en parties pour extraire le type de m�dia et les donn�es base64
        string[] parts = dataUrl.Split(',');
        string mediaType = parts[0].Split(':')[1].Split(';')[0];
        string base64Data = parts[1];

        // Convertissez les donn�es base64 en tableau d'octets
        byte[] imageData = System.Convert.FromBase64String(base64Data);

        // Cr�ez une texture � partir des octets
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        // Cr�ez un sprite � partir de la texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Utilisez le sprite comme vous le souhaitez, par exemple, affectez-le � un objet SpriteRenderer
        image.sprite = sprite;
    }

    private void addOnePlayerWord(string ID, string PSEUDO, Color COULEUR, string MOTS)
    {
        PlayerWord joueurConnecte = new PlayerWord(ID, PSEUDO, COULEUR, MOTS);
        listeJoueurs.Add(joueurConnecte);
    }
    private void addOnePlayerPicture(string ID, string PSEUDO, Color COULEUR, string IMGHEXA, bool isDraw)
    {
        PlayerPicture joueurConnecte = new PlayerPicture(ID, PSEUDO, COULEUR, IMGHEXA, isDraw);
        listeJoueurs.Add(joueurConnecte);
    }
    private void addOnePlayerVideo(string ID, string PSEUDO, Color COULEUR, string VIDEO)
    {
        PlayerVideo joueurConnecte = new PlayerVideo(ID, PSEUDO, COULEUR, VIDEO);
        listeJoueurs.Add(joueurConnecte);
    }

    private void removeOnePlayer(string id)
    {
        Player joueur = GetPlayerById(id);
        if (joueur != null)
        {
            listeJoueurs.Remove(joueur);
        }



    }

    private string GetNameById(string id)
    {
        Player joueur = GetPlayerById(id);
        if (joueur != null)
        {
            return joueur.Pseudo;
        }

        return null;
    }

    private Player GetPlayerById(string id)
    {
        foreach (Player joueur in listeJoueurs)
        {
            if (joueur.Id == id)
            {
                return joueur;
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

    public List<Player> getListeJoueurs()
    {
        return listeJoueurs;
    }

    public void gameStarted()
    {
        isGamePlaying = true;
        foreach (Player joueur in listeJoueurs)
        {
            ws.Send(joueur.Id + ":GAME START");

        }

    }

    public void askPlayerToAnswer()
    {
        isPlayerAnswering = true;
        foreach (Player joueur in listeJoueurs)
        {
            ws.Send(joueur.Id + ":ANSWER");


        }

    }

    public void askPlayerToVote()
    {
        isPlayerAnswering = false;
        isPlayerVoting = true; ;
        string message = "";
        System.Random rand = new System.Random();
        List<Player> listeAleatoire = listeJoueurs.OrderBy(joueur => rand.Next()).ToList();

        foreach (Player joueur in listeAleatoire)
        {
           
            message += ","; 
            message += joueur.Id;
            message += ",";
            message += joueur.answer; //si fais en un message, il y a des probl�mes de dll fuck up qui sont lier a des probl�mes de virus donc laisser ca comme ca pour le moment
        }

        foreach (Player joueur in listeJoueurs)
        {
            ws.Send(joueur.Id + ":VOTE," + joueur.answer + message);

        }

    }






}
