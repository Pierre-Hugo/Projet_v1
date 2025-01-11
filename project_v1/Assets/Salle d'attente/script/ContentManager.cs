using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using System.Collections.Generic;

using System.Linq;
using UnityEngine.Video;


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
        boutonStart.interactable = true; // devrais être a false en temps normale
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
           // id = "28EP"; // à enlver en temps normal
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



        addOnePlayerPicture("USER1234", "jf", Color.red, "data:image/png;base64,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",true);
        addOnePlayerPicture("USERABCD", "peach", Color.cyan, "data:image/png;base64,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", true);
        addOnePlayerPicture("USER5678", "Simone", Color.blue, "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAMAAACahl6sAAADAFBMVEX///9MTExMTExMTExMTExMTExMTExMTExMTExMTExMTExMTExMTExMTExMTEzMzMzExMS8vLy0tLSsrKykpKSZmZmUlJSMjIyEhIR8fHxzc3NsbGxmZmZaWlpSUlJMTEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAdCos8AAAAD3RSTlMAESIzRFVmd4iZqrvM3e5GKvWZAAAKxUlEQVR42u1daXfaOhqWZNkQ2py5pb3tnTP//5/NnM4kLP2QsFjbAAmWbL+WtdGIe6wPLQQj6dHz7pINQlOb2tSmNrWpTW1qU5vaSMP3Ms8CUYQJwl8vb7cCCcnlXQHBtCDkK/jRlnF2J0BwWX6zXrCpj3cABM+qL6MXbXYidyD0Yel03c86byDVP12vPCMpssXx8JfzpY+vEpFcccy/+4DG2TJS/eXF3oFnykgx98R9cpd5KghgdvnJzIorUEQwMUj4csgTyBzy5Ero1+IMiZRaMYosRav4Af61+wd5bAIURLIEYipIrecKSFvz6luOQCojvKr3FkqQQjaYHx5hGYSInaEZfSAkayAzw2LtTPEhFrXZ5AcEV/r1QbSAdCdbaJsrac6EyMPpH2aITyslJAYOxGnOhLwpOiuv70v6xskJD+6E7fkBMQh5z2N5A+Q6+74+rATJWUPe/mMOXztmZ7VKTUj9ruZSjn5rxbMDMusR4kDJdp+dHyl1ll43RIwysjtdkZmyV1CQVY3w8Xp2/nkBITrK4o0jrOxZ7PP+EnHRXAk5grFwjw3G3gHnCkQ2Gl6ZaryR1+ovWisphY5fsgJi1BWPEEur3VmKXmCpzMpm6USjUXWil3q7Uxb1yinD1arOFKQ2Ct0HkBLZJatGdwJEz1kIoDrypO4ECKjqFKo0ZA5Ez1kxgKUtuxMg5E9A1UnhpiE5AQG9uqH/7F6A6DlLAaDbiDsBQpcAIe6SlQ+QEvIX7pKVDRD8HVB1I19cyTsBAhJieMPRdNcl+r1W+LA1w1EiYP5NYQEK4E1DFgbkNOPz5E/zJsvfzo0sgYDRHp70gOCCnHe0lh8qZBRaW+4eF5CyoF9Qpm3LXIGQ2XeUcauRI5D5D5R1Y25A8OJb3jg2DgbxbFlzx+FUxj4BmeeOw0VFTkDIj9xxrCVy0ZGZR5fcUuTXhbRL+XloBx8XNyHkBKQEo43zfC9eSCj0R/PBi60nDcTmvuYNkNqYIMGYKGVaUkK8VATR0vSCgp+SGtENB0QzdiGi5QQbg9mu+0fz6lk5ATGYFjt4ngqYRXDT41kXpcR+hJhA5MsA9KRAtMRIt7B+6waE6LkNViRthyhuBMTYE2WO/X51ozpVc5UsT5vVyhC5xeh6pGEjSw0ILGTLvcKTtKkudQJCnVIMYy+BoXRAUuoIcWLEX7IQ2Y5fk9JqEScdmfmFJ5eOXUQwIRLqAsSontQooWh5ypbbvpJSLqrurCImI3H7om521U2yKs/w5NKzdCDHxf7idJLlHZ4kZSShX/cOT5yBSD+VihQt//DkredV85LcHoiLaAU4kcvMHChxMb/ExWU7GS3/8OStaw4thb/5deLKxbQFhCdvLBhALuVje5kVqxjJKm4mWWdZWfzZVwrZlpJOYQGSxBvcLrB+8dM+YHGus6I2p3vz5kPIWbQZyrQxT6lVYpEljifmyUiulHD09wCyYd5A5PPda8i7H+M5Aqn9Lr+4KKEessOxPoZEcQaNApNbqaz2mUZJf6g676u4tEsj3xu9F9cgUd+6yBr/2S52f25e/Roa65OODw7wFfN5qGS9AxGrZteqOpzmp3h3ORsg4pAg0uIJc9x20KqJxKU9/h3NIwfDQTIa6VMSLFlXILUub819KyMdfCo89HU+wGhJI2ooHwhiJDyGN4ShRqFADGM3C2EkRZobVD3pAlFPuj9yG0pG69dastYiGIg7JThasgYYCcxxu0DkSq8MDkvbI43vDEWoCILKxZAFVtEVoXEVCXcirWmxjc0C84RAYEZKp4KSSxXnaLPAakS0RuGRsSKjUSit44CwrUXdxYiyE3dC4EMgYYVScAKqtllg6Wp/eZiuV1Gq3l7J2lb1kZH2d0xFtBBsRFz/55S3KdXNejGuaK4svKh/zwjMtOOtp/Y5YUriCGnLiREDz4ctR0HbZcXLGwLlFHBbQOJIIiWrIyePH3vk982J7FCsjuRRGAqcA4mpXNyibZIAyaDCFSoUBGVGSegMOgGHlB9c4Xqu0zDy4ZQEm5vMgGyCgXQDJ/WU9LaFd/ftXGMMt//U0lUnTG3Kjka1sZlr1URnB6CC96hzBaCku6gSCEQPCN9o797ejPzUZAz8ON5PS36toa8RwD9JlEpHWhWuMlHaTruiNhTAR0QWNiCdYF4EZ7v2EN44LsdTAhmscDmnVpYsFphpdAA/uLhDlAgUKFuFtYJoHD1JDESsB3J3HgjEmq6TKoWGwOJeQwYlQkmskmVU4I+pgbCBPYZAJcHWUqgmnanUQNTAHkOg/bU+f8Ioz0YRAguJoXRzUEe8Nky1ZIn+muv+VzI9EKktMI2nxCZZFUmj6kNqy2BKZAgQ681ShqrXtwDCVyAlQbs9pSU+MTqPzR/IaH7zEOlIbMZ3jm4NpNZ7DMWcXhdOWHMJf+Nb0lSqPiwjx86ySc6ZUrLB7XwjH/QgjaJ8W5YFSqXqw0DqzbITSVSI1bzyli3aMb64LGm/DL6pbwUE+nupn8bgru3tvXM6K9PWHEYmhGfwncjYW9uN1eeIzgfwb463AVI8fB3/IvckRMrF8FlbEn3vILRrU3xyeIpFp8agNxNOHzQ+sMCzZgCJbTtE/zukB+KEo+vdog9ir3YqMRA3HOnb+jXRsfsrssUHbfYsOEsKZN458i8VlxL/jl+OWTCWULTov1oohHgPHYoi5S1YUqrLL1Yk1PgukM+G4VUtskmV6PipEtdf0iKEtvv8D08lWpVRwpZ1yzApnuYgLaubRFFJLglufZYKyIOO42TvRv0UR4LlsW22FVcGlMVepAFS6F/GUcADB+KRiFr19cUs4AVTQgcyT1RDVp0N+71VM6927sLNzXsOzVPWuiS03IkkQMBnJZqCwPSYT3tHzjUOCdtXoX90AFX7QCCtFS6+jMXVwr9IV46wfB4svD4OA6EjhLQALrEvEDaYzmqruyRJGAEKP11Kxo7S9QZYWooPwb2OAHG5Jdj33BaBZtvTPZkWiPbqygFI4WlNbHWS2BrKYDlI3mBINyApGCkSLhDQqUK3bGQk7U0Z8v42IDeG5GZYRYLeDYNEbsADjgbpDGTroHK+B7/dPASJFkACO1gXIMoTiGXVcZFUtMxt2yEkmHgOKVwqRsYPQYi0jAw+/0HnDiu3ATgaDwgJ9ZTXUUbWo5SU3jKg+6RDNFcxN1eBgsvAzk0c2HvtRvs0d0S3iYAYtyMaVVvjauMmjwAgYJ+oMkQuvPhAOhmg8Ul/VBJyG5Sx133qoKfwxNxqiDjF0en4sLWMSk1o7mt3bC1F2VoeXLYGqcPjsa7+4c+mdceGeyKVOQePJ0uoY6tPqh9aWpRVYeJa74Jx9OOGz609nmtVkJCiBXn74uGB8WOnvn9Ccv7V656Y/Ttiu6dn23fEHBVTeg4aekPWPpGEeu0AwRh08z9jtq16HcoevaTvktd+1Wbx0+Wqp6id3b6PkvvHsS9tXzxHEeN9OtfJnIGMj7p99Q5RxZ6P3D0Qu4kIRQ3iVTxYcQTIsuDM9mCf7a/YDWow/JHMMur6NShlUPWBDS7Pahf9aJahtG02G9gS/W+408IV3OnmkODeLuw36tMxroRQ0d5TyZ7rJA/KsSXSVdl+NP6G1QkqIbQo8Hu/a8Vlqhvt7PUSTE85z3nUjZSS/5ZHNE9talOb2tSmNrWpTW1qN2v/BwimbnbOwRpLAAAAAElFTkSuQmCC", true);
        addOnePlayerPicture("USER5671", "Simon", Color.blue, "data:image/png;base64,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", true);
        addOnePlayerPicture("USER5672", "Simoe", Color.blue, "data:image/png;base64,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", true);
        addOnePlayerPicture("USER5673", "Simne", Color.blue, "data:image/png;base64,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", true);

        //addOnePlayerWord("USER4678", "Simon", Color.black, "Allo");





        //addOnePlayerWord("USEREFGH", "Flex", Color.green, "Bonjour");
        //listScript.AjouterListe("Flex", Color.green);
        //listScript.AjouterListe("Simone", Color.blue);
        //listScript.AjouterListe("JF", Color.red);
        //listScript.AjouterListe("PEACH", Color.cyan);

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
                    int firstColounIndex = dataRecu.Data.IndexOf(':');
                    string idRecu = dataRecu.Data.Substring(0, firstColounIndex);
                    string[] messageRecu = dataRecu.Data.Substring(firstColounIndex + 1).Split(",");

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
                                string typePlayer = messageRecu[3];


                                //vérifie si le type de joueur est valide
                                if (typePlayer != "PIC" && typePlayer != "VID" && typePlayer != "WRD")
                                {
                                    ws.Send(idRecu + ":Donne invalides");
                                    donneValide = false;

                                    break;
                                }

                                foreach (Player joueur in listeJoueurs)
                                {
                                    //vérifie si l'id, le pseudo ou la couleur est déjà utilisé
                                    if (joueur.Id == idRecu || joueur.Pseudo == pseudoRecu )
                                    {
                                        ws.Send(idRecu + ":Donne invalides");
                                        donneValide = false;

                                        break;
                                    }

                                }
                                if (donneValide)
                                {

                                    
                                    switch (typePlayer)//ajoute le bon type de Player
                                    {
                                        case "PIC": // exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,PIC,TRUE,(code de l'image)"

                                            string img = messageRecu[5] + "," + messageRecu[6];
                                            bool isDraw = messageRecu[4] == "TRUE";
                                            addOnePlayerPicture(idRecu, pseudoRecu, couleurRecu, img, isDraw);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;

                                        case "VID": //exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,VID,(code de la video)"
                                            string vid = messageRecu[4] +","+ messageRecu[5];
                                            addOnePlayerVideo(idRecu, pseudoRecu, couleurRecu, vid);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;

                                        case "WRD": //exemple de message:  messageRecu = "NP,Xx_coolGuy_xX,BLUE,WRD,Coucou"
                                            string word = messageRecu[4];
                                            addOnePlayerWord(idRecu, pseudoRecu, couleurRecu, word);
                                            listScript.AjouterListe(pseudoRecu, couleurRecu);
                                            break;
                                        default:
                                            ws.Send(idRecu + ":Donne invalides");
                                            break;
                                    }


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
                            //Si la partie est déjà commencer, je vais mettre le joueur comme connecter
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
                        if (joueur != null && idRecu != vote)
                        {
                            joueur.vote = vote;
                        }
                    }

                    //message envoyer par le serveur
                    //exemple de message recu
                    //string[] messageRecu = "Client ID not found"
                    else if (instruction == "Client ID not found")
                    {
                        //ce message va etre recu si on envoye un message à un id qui est introuvable, donc quelqu'un de déconnecté, mais qui l'a déjà été
                        //Je met donc son état isConnected à false pour le moment et il peux se reconnecter plus tard
                        //normalement, cela ne peux arriver que quand la partie est commencé
                        Player joueur = GetPlayerById(idRecu);
                        if (joueur != null)
                        {
                            joueur.PlayerConnected(false);
                        }

                    }

                    else if (instruction == "CHECK")
                    {
                        if (listeJoueurs.Count >= nbMaxJoueurs)
                        {
                            ws.Send(idRecu + ":NO");
                        }
                        else
                        {
                            ws.Send(idRecu + ":YES");
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




    private void addOnePlayerWord(string ID, string PSEUDO, Color COULEUR, string MOTS)
    {
        PlayerWord joueurConnecte = new PlayerWord(ID, PSEUDO, COULEUR, MOTS);
        listeJoueurs.Add(joueurConnecte);
    }
    private void addOnePlayerPicture(string ID, string PSEUDO, Color COULEUR, string IMG, bool isDraw)
    {
        PlayerPicture joueurConnecte = new PlayerPicture(ID, PSEUDO, COULEUR, IMG, isDraw);
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

    public Player GetPlayerById(string id)
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
            foreach (Player joueur in listeJoueurs)
            {
                ws.Send(joueur.Id + ":CLOSE");
            }

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
            ws.Send(joueur.Id + ":START");
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

        System.Random rand = new System.Random();
        List<Player> listeAleatoire = listeJoueurs.OrderBy(joueur => rand.Next()).ToList();

        foreach (Player joueur in listeJoueurs)
        {
            ws.Send(joueur.Id + ":VOTE");
        }

        System.Threading.Thread.Sleep(1000);
        foreach (Player joueur in listeJoueurs)
        {
           
            string message = "";

            foreach (Player player in listeJoueurs)
            {
                if (player != joueur)
                {
                    message += player.Id +","+ player.answer+ ",";
                }
            }
            message = message.Remove(message.Length - 1, 1);

            ws.Send(joueur.Id + ":" + message);
        }
    }


    
    
}


