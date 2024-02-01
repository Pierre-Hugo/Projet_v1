using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.Animations;
using System.Collections;
using UnityEngine.Networking;

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
    public GameObject test;
    private bool isGamePlaying;
    private string url;
    public Image image;
    


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
        url = "data:image/jpeg;base64,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\r\n";


        LoadSpriteFromDataUrl(url);




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

        addOnePlayerWord("12345", "jf", Color.red,"Salut");
        addOnePlayerWord("67890", "peach", Color.cyan,"Coucou");
        listScript.AjouterListe("JF", Color.red);
        listScript.AjouterListe("PEACH", Color.cyan);
        
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
                    //string[] messageRecu = "NP,Xx_coolGuy_xX,BLUE,[...]"
                    if (instruction == "NP" && !isGamePlaying)
                    {
                        if (listeJoueurs.Count < nbMaxJoueurs && messageRecu.Length>4)
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
                                
                                string typePlayer = messageRecu[3];
                                switch (typePlayer)//ajoute le bon type de Player
                                {
                                    case "PIC": // exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,PIC,(code de l'image), TRUE"
                                        if (messageRecu.Length > 5)
                                        {
                                            break;
                                        }
                                        string img= messageRecu[4];
                                        bool isDraw = messageRecu[5]=="TRUE";
                                        addOnePlayerPicture(idRecu, pseudoRecu, couleurRecu,img,isDraw);
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
                            ws.Send(idRecu + ": Salle Pleine");
                        }
                    }

                    //DC pour Disconnected
                    //exemple de message recu
                    //string[] messageRecu = "DC"
                    else if (instruction == "DC")
                    {
                        foreach (Player joueur in listeJoueurs)
                        {
                            if (joueur.Id == idRecu) // chercher l'id dans la liste qui correspond à celui recu
                            {
                                removeOnePlayer(joueur.Id);
                                listScript.retirerListe(joueur.Pseudo);
                                if (listeJoueurs.Count <= 2)
                                {
                                    boutonStart.interactable = false;
                                }
                                break;
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

                    //AN pour Change Answer
                    //exemple de message recu
                    //string[] messageRecu = "AN,Chien"
                    else if (instruction == "AN" && isGamePlaying)
                    {
                        string reponse = messageRecu[1];
                        foreach (Player joueur in listeJoueurs)
                        {
                            if (joueur.Id == idRecu)
                            {
                                joueur.answer = reponse;
                            }
                        }
                    }

                    else
                    {
                        ws.Send(idRecu + ": Impossibe de traiter la demande");
                    }
                    
                    listeDataRecu.Remove(dataRecu);
                }
            }
        }

        
    }


    void LoadSpriteFromDataUrl(string dataUrl)
    {
        // Divisez l'URL en parties pour extraire le type de média et les données base64
        string[] parts = dataUrl.Split(',');
        string mediaType = parts[0].Split(':')[1].Split(';')[0];
        string base64Data = parts[1];

        // Convertissez les données base64 en tableau d'octets
        byte[] imageData = System.Convert.FromBase64String(base64Data);

        // Créez une texture à partir des octets
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        // Créez un sprite à partir de la texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Utilisez le sprite comme vous le souhaitez, par exemple, affectez-le à un objet SpriteRenderer
        image.sprite = sprite;
    }

    private void addOnePlayerWord(string ID, string PSEUDO, Color COULEUR,string MOTS)
    {
            PlayerWord joueurConnecte = new PlayerWord(ID, PSEUDO, COULEUR,MOTS);
            listeJoueurs.Add(joueurConnecte);
    }
    private void addOnePlayerPicture(string ID, string PSEUDO, Color COULEUR, string IMGHEXA, bool isDraw)
    {
        PlayerPicture joueurConnecte = new PlayerPicture(ID, PSEUDO, COULEUR,IMGHEXA,isDraw);
        listeJoueurs.Add(joueurConnecte);
    }
    private void addOnePlayerVideo(string ID, string PSEUDO, Color COULEUR,string VIDEO)
    {
        PlayerVideo joueurConnecte = new PlayerVideo(ID, PSEUDO, COULEUR,VIDEO);
        listeJoueurs.Add(joueurConnecte);
    }

    private void removeOnePlayer(string id)
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
    }

    public void askAnswerToPlayer()
    {
        foreach (Player joueur in listeJoueurs)
        {
            ws.Send(joueur.Id + ":Answer");
        }
        
    }



    


}
