using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BaseScenario : MonoBehaviour
{
    protected List<Player> listeJoueurs;
    protected List<Player> listeJoueursAleatoire;
    protected ContentManager scriptPrincipal;
    public Text Timer;
    protected float timer;
    protected float tempsAttente;
    protected bool questionAsk;
    protected int playerShow;
    public GameObject modeleReponse;
    public GameObject Question;
    protected Liste listScript;
    protected Player joueurChoisi;
    public Vector2 postionReponse;
    public Vector2 dimensionReponse;
    protected float tempsPourRepondre;
    protected float tempsPourVoter;
    protected float tempsAfficherReponse;
    protected bool showTimer;

    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        Transform parentOfParent = parent.parent;
        scriptPrincipal = parentOfParent.GetComponent<ContentManager>();
        questionAsk = false;
        playerShow = 0;
        tempsAttente = 10f;
        timer = 0f;
        listScript = FindObjectOfType<Liste>();
        tempsPourRepondre = 30f;
        tempsPourVoter = 15f;
        tempsAfficherReponse = 8f;
        showTimer = false;
    }

    // Update is called once per frame


    public void initialisation(List<Player> Joueurs)
    {

        listeJoueurs = Joueurs;
        System.Random rand = new System.Random();
        listeJoueursAleatoire = listeJoueurs.OrderBy(joueur => rand.Next()).ToList();
    }
    public void afficherReponse(string reponse, Vector2 position, Vector2 dimension)
    {
        GameObject cadreReponse = null;
        // Trouver tous les GameObjects de type "GameObject" dans la scène
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Rechercher un GameObject dont le nom commence par "Background-Reponse"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Background-Reponse"))
            {
                // Trouvé le GameObject qui commence par "Background-Reponse"
                cadreReponse = obj;
                break;  
            }
        }

        if (cadreReponse == null)//vérifie si une réponse est déja afficher
        {
            cadreReponse = Instantiate(modeleReponse, transform);
            cadreReponse.GetComponent<RectTransform>().anchoredPosition = position;
            cadreReponse.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = dimension;
        }


        cadreReponse.transform.GetChild(0).GetComponent<Text>().text = reponse;
    }

    protected void afficherReponses()
    {
        // Trouver tous les GameObjects de type "GameObject" dans la scène
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Rechercher un GameObject dont le nom commence par "Background-Reponse"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Background-Reponse"))
            {
                // Trouvé le GameObject qui commence par "Background-Reponse"
                Destroy(obj);
                break;
            }
        }

        

        foreach (Player joueur in listeJoueursAleatoire)
        {
            listScript.AjouterListe(joueur.answer, Color.white);
            joueur.answer = "";
        }


        scriptPrincipal.askPlayerToVote();
    }


    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (showTimer)
        {
            int tempsRestant = (int)tempsAttente - (int)timer;
            if (Timer.text != tempsRestant.ToString())
            {
                Timer.text = tempsRestant.ToString();
            }
        }

        if (timer >= tempsAttente)
        {
            if (!questionAsk)
            {
                scriptPrincipal.askPlayerToAnswer();
                questionAsk = true;
                tempsAttente = tempsPourRepondre;
                timer = 0f;
                showTimer = true;
            }
            else
            {
                if (Question != null)
                {
                    Destroy(Question);
                    showTimer = false;
                    Destroy(Timer);
                }

                if (listeJoueurs.Count == playerShow) //affiche tout les réponse et demande au joueurs de voter pour une réponse
                {
                    afficherReponses();
                    tempsAttente = tempsPourVoter;
                    timer = 0f;
                }
                else if (listeJoueurs.Count <= playerShow) //ajoute les points et met fin au scénario
                {
                    Destroy(gameObject);
                }
                else //affiche la réponse d'un joueur
                {
                    timer = 0f;


                    bool JoueurValide = false;
                    if (joueurChoisi == null)
                    {
                        JoueurValide = true;
                    }
                    foreach (Player joueur in listeJoueursAleatoire)
                    {
                        if (JoueurValide)
                        {
                            joueurChoisi = joueur;
                            JoueurValide = false;
                            afficherReponse(joueur.answer, postionReponse, dimensionReponse); //modifier l'emplacement des réponses
                            break;
                        }
                        else if (joueurChoisi == joueur)
                        {
                            JoueurValide = true;
                        }
                    }
                    if (JoueurValide)
                    {

                    }


                    tempsAttente = tempsAfficherReponse;

                }

                playerShow++;
            }
        }

    }
    protected void OnDestroy()
    {
        PlayingScript scriptJeu = GetComponentInParent<PlayingScript>();
        if (scriptJeu != null)
        {
            scriptJeu.callScoreBoard();
        }
    }

    protected void startTimer(int temps)
    {

    }

}
