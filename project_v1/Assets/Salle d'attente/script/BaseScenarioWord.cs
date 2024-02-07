using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class BaseScenarioWord : MonoBehaviour
{
    public Text textRecu;
    protected List<Player> listeJoueurs;
    protected ContentManager scriptPrincipal;
    protected float timer;
    protected float tempsAttente;
    protected bool questionAsk;
    protected int playerShow;
    protected List<Player> listeJoueursDejaAfficher;
    public GameObject modeleReponse;
    public GameObject Question;
    private Liste listScript;

    void Start()
    {
        Transform parent = transform.parent;
        Transform parentOfParent = parent.parent;
        scriptPrincipal = parentOfParent.GetComponent<ContentManager>();
        questionAsk = false;
        playerShow = 0;
        listeJoueursDejaAfficher = new List<Player>();
        tempsAttente = 10f;
        timer = 0f;
        listScript = FindObjectOfType<Liste>();
    }

    

    public void initialisation(string motPlaque, List<Player> Joueurs)
    {
        textRecu.text = motPlaque;
        listeJoueurs = Joueurs;
    }
    public void afficherReponse(string reponse, Vector2 position, Vector2 dimension)
    {
        GameObject cadreReponse = GameObject.Find("Background-Reponse(Clone)");


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
        GameObject cadreReponse = GameObject.Find("Background-Reponse(Clone)");

        Destroy(cadreReponse);

        listeJoueursDejaAfficher = new List<Player>();
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
                    foreach (Player joueurDejaChoisi in listeJoueursDejaAfficher)
                    {
                        if (joueur == joueurDejaChoisi) // vérifie si le joueur à déja été choisi
                        {
                            JoueurValide = false;
                            break;
                        }
                    }

                    if (JoueurValide)
                    {
                        listeJoueursDejaAfficher.Add(joueur);
                        listScript.AjouterListe(joueur.answer, Color.white);
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
        } while (listeJoueurs.Count != listeJoueursDejaAfficher.Count); // recommence si le joueur choisi avait déjà été choisi

        scriptPrincipal.askPlayerToVote();
    }

    protected void givePoints() 
    {

    }



    private void OnDestroy()
    {
        PlayingScript scriptJeu = GetComponentInParent<PlayingScript>();
        if (scriptJeu != null)
        {
            //scriptJeu.callNewScenario();        Enlever le commentaires SEULEMENT quand la sélection du scénario ne va pas être hardcode sinon il va boucler à l'infini
        }
    }
}
