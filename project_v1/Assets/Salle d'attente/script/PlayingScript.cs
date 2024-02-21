using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;




public class PlayingScript : MonoBehaviour
{
    public enum TypeScenario
    {
        Word ,
        Picture,
        Draw ,
        Video
    }

    public GameObject ScenarioWord1;
    public GameObject ScenarioWord2;
    public GameObject ScenarioWord3;
    public GameObject ScenarioWord4;
    public GameObject ScenarioWord5;
    public GameObject ScenarioPicture1;
    public GameObject ScenarioPicture2;
    public GameObject ScenarioPicture3;
    public GameObject ScenarioPicture4;
    public GameObject ScenarioPicture5;
    public GameObject ScenarioDraw1;
    public GameObject ScenarioDraw2;
    public GameObject ScenarioDraw3;
    public GameObject ScenarioDraw4;
    public GameObject ScenarioDraw5;
    public GameObject ScenarioVideo1;
    public GameObject ScenarioVideo2;
    public GameObject ScenarioVideo3;
    public GameObject ScenarioVideo4;
    public GameObject ScenarioVideo5;

    public GameObject ScoreBoard;
    private bool[,] ScenarioDejaJouer;
    private List<Player> listeJoueurs;
    private List<Player> listeJoueursDejaJouer;
    int nbTypeScenario;
    int nbScenarioParType;
    Player joueurChoisi;
    void Start()
    {
        nbTypeScenario = 4;
        nbScenarioParType = 5;
        ContentManager contentManager = FindObjectOfType<ContentManager>();
        listeJoueurs=contentManager.getListeJoueurs();
        listeJoueursDejaJouer = new List<Player>();
        ScenarioDejaJouer = new bool[nbTypeScenario,nbScenarioParType];

        //initiation du tableau
        for (int i = 0; i== nbTypeScenario; i++)
        {
            for (int j = 0; j == nbScenarioParType; j++)
            {
                ScenarioDejaJouer[i,j] = false;
            }
        }

        callNewScenario();
    }
    public void callNewScenario()
    { 
        if(listeJoueursDejaJouer.Count == listeJoueurs.Count)
        {
            SceneManager.LoadScene("SalleAttente");
        }

        System.Random random = new System.Random();
        int randomIndex;
        bool JoueurValide;
        do
        {
            JoueurValide = true;
            
                randomIndex = random.Next(0, listeJoueurs.Count);
            
            int i = 0;
          

            foreach (Player joueur in listeJoueurs)
            {
                if (i == randomIndex)//prend un joueur aléatoire dans la liste
                {
                    foreach (Player joueurDejaChoisi in listeJoueursDejaJouer)
                    {
                        if (joueur == joueurDejaChoisi) // vérifie si le joueur à déja été choisi
                        {
                            JoueurValide = false;
                            break;
                        }
                    }

                    if (JoueurValide)
                    {
                        joueurChoisi = joueur;
                        listeJoueursDejaJouer.Add(joueurChoisi);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }
        } while (!JoueurValide); // recommence si le joueur choisi avait déjà été choisi

        bool ScenarioValide = false;

        do
        {
            randomIndex = 0;//random.Next(0, nbScenarioParType);

            if (joueurChoisi is PlayerPicture)
            {
                // Différencié si c'est un dessein ou une photo
                PlayerPicture player = joueurChoisi as PlayerPicture;
                if (player.isDraw)
                {
                    if (!ScenarioDejaJouer[(int)TypeScenario.Draw, randomIndex])
                    {
                        GameObject scenario;
                        ScenarioValide = true;
                        PlayerPicture joueurChoisiPicture = (PlayerPicture)joueurChoisi;
                        BaseScenarioPicture scriptScenario = null;
                        ScenarioDejaJouer[(int)TypeScenario.Draw, randomIndex] = true;

                        switch (randomIndex)
                        {
                            case 0:
                                scenario = Instantiate(ScenarioDraw1, transform);
                                scriptScenario = scenario.GetComponent<ScDraw1Script>();
                                break;
                            case 1:
                                //Instantiate(ScenarioDraw2, transform);
                                break;
                            case 2:
                                //Instantiate(ScenarioDraw3, transform);
                                break;
                            case 3:
                                //Instantiate(ScenarioDraw4, transform);
                                break;
                            case 4:
                                //Instantiate(ScenarioDraw5, transform);
                                break;
                        }
                        scriptScenario.initialisation(joueurChoisiPicture.img, listeJoueurs);
                    }

                }
                else
                {
                    if (!ScenarioDejaJouer[(int)TypeScenario.Picture, randomIndex])
                    {
                        GameObject scenario;
                        ScenarioValide = true;
                        PlayerPicture joueurChoisiPicture = (PlayerPicture)joueurChoisi;
                        BaseScenarioPicture scriptScenario = null;
                        ScenarioDejaJouer[(int)TypeScenario.Picture, randomIndex] = true;

                        switch (randomIndex)
                        {
                            case 0:
                                scenario=Instantiate(ScenarioPicture1, transform);
                                scriptScenario = scenario.GetComponent<ScPicture1Script>();
                                break;
                            case 1:
                                //Instantiate(ScenarioPicture2, transform);
                                break;
                            case 2:
                                //Instantiate(ScenarioPicture3, transform);
                                break;
                            case 3:
                                //Instantiate(ScenarioPicture4, transform);
                                break;
                            case 4:
                                //Instantiate(ScenarioPicture5, transform);
                                break;
                        }
                        scriptScenario.initialisation(joueurChoisiPicture.img, listeJoueurs);
                    }
                }
            }
            else if (joueurChoisi is PlayerVideo)
            {
                if (!ScenarioDejaJouer[(int)TypeScenario.Video, randomIndex])
                {
                    ScenarioValide = true;
                    ScenarioDejaJouer[(int)TypeScenario.Video, randomIndex] = true;

                    switch (randomIndex)
                    {
                        case 0:
                            //Instantiate(ScenarioVideo1, transform);
                            break;
                        case 1:
                            //Instantiate(ScenarioVideo2, transform);
                            break;
                        case 2:
                            //Instantiate(ScenarioVideo3, transform);
                            break;
                        case 3:
                            //Instantiate(ScenarioVideo4, transform);
                            break;
                        case 4:
                            //Instantiate(ScenarioVideo5, transform);
                            break;
                    }
                }
            }
            else if (joueurChoisi is PlayerWord)
            {
                if (!ScenarioDejaJouer[(int)TypeScenario.Word, randomIndex])
                {
                    GameObject scenario;
                    ScenarioValide = true;
                    PlayerWord joueurChoisiMot = (PlayerWord)joueurChoisi;
                    BaseScenarioWord scriptScenario = null;
                    ScenarioDejaJouer[(int)TypeScenario.Word, randomIndex] = true;
                    switch (randomIndex)
                    {
                        case 0:
                            scenario = Instantiate(ScenarioWord1, transform);
                            scriptScenario = scenario.GetComponent<ScWord1Script>();
                            
                            break;
                        case 1:
                            //Instantiate(ScenarioWord2, transform);
                            break;
                        case 2:
                            //Instantiate(ScenarioWord3, transform);
                            break;
                        case 3:
                            //Instantiate(ScenarioWord4, transform);
                            break;
                        case 4:
                            //Instantiate(ScenarioWord5, transform);
                            break;
                    }
                    scriptScenario.initialisation(joueurChoisiMot.word,listeJoueurs);
                }
            }
        } while (!ScenarioValide);
    }

    public void callScoreBoard()
    {
        GivePlayersPoints();
        Instantiate(ScoreBoard, transform);
    }

    private void GivePlayersPoints()
    {
        foreach (Player player in listeJoueurs) // reset les vote
        {
            player.nbVote = 0;

        }
        ContentManager contentManager = FindObjectOfType<ContentManager>();
        
        foreach (Player player in listeJoueurs)
        {
            if (player.vote != "")//vérifie que le joueur a voté
            {
                contentManager.GetPlayerById(player.vote).nbVote++; // donne un vote au joueurs
                player.vote = "";
            }
        }

        foreach (Player player in listeJoueurs)
        {
            switch (player.nbVote)
            {
                case 1:
                    player.AddPoints(100);
                    break;
                case 2:
                    player.AddPoints(225);
                    break;
                case 3:
                    player.AddPoints(360);
                    break;
                case 4:
                    player.AddPoints(500);
                    break;
                case 5:
                    player.AddPoints(575);
                    break;

            }
        }


    }

}
