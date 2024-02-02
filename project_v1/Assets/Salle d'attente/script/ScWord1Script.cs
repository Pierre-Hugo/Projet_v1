using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScWord1Script : MonoBehaviour
{
    public Text textPlaque;
    private List<Player> listeJoueurs;
    ContentManager scriptPrincipal;
    private float timer;
    private float tempsAttente;
    private bool questionAsk;
    private int playerShow;
    private List<Player> listeJoueursDejaAfficher;
    public GameObject modeleReponse;
    

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
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if(timer>=tempsAttente )
        {
            if (!questionAsk)
            {
                scriptPrincipal.askAnswerToPlayer();
                questionAsk = true;
                tempsAttente = 15f;
                timer = 0f;
            }
            if (questionAsk)
            {
                if (textPlaque !=null)  
                {
                Destroy(textPlaque);
                }

                if(listeJoueurs.Count == playerShow) //affiche tout les réponse et demande au joueurs de voter pour une réponse
                {
                    //show all anwser
                    tempsAttente = 10f;
                }
                else if(listeJoueurs.Count <= playerShow) //ajoute les points et met fin au scénario
                {
                    //ajouter les points selon les votes
                    Destroy(this);
                }
                else //affiche la réponse d'un joueur
                {
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
                                    afficherReponse(joueur.answer);
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

                     tempsAttente = 7f;
                }

                playerShow++;
            }
            
        }
    }

    public void initialisation(string motPlaque, List<Player> Joueurs) 
    {
        textPlaque.text = motPlaque;
        listeJoueurs = Joueurs;
    }

    public void afficherReponse(string reponse)
    {
        GameObject cadreReponseExistant = GameObject.Find("Background-Reponse");
        GameObject cadreReponse;

        if (cadreReponseExistant == null)
        {
            cadreReponse = Instantiate(modeleReponse, transform);
            cadreReponse.transform.position = new Vector3(370f, 0f, 0f);
        }
        else
        {
            cadreReponse = cadreReponseExistant;
        }

        cadreReponse.transform.Find("Reponse").GetComponent<Text>().text = reponse;
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
