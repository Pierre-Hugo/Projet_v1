using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class BaseScenarioWord : MonoBehaviour
{
    public Text textRecu;
    protected List<Player> listeJoueurs;
    protected List<Player> listeJoueursAleatoire;
    protected ContentManager scriptPrincipal;
    protected float timer;
    protected float tempsAttente;
    protected bool questionAsk;
    protected int playerShow;
    public GameObject modeleReponse;
    public GameObject Question;
    private Liste listScript;
    protected Player joueurChoisi;
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
    }



    public void initialisation(string mot, List<Player> Joueurs)
    {
        textRecu.text = mot;
        listeJoueurs = Joueurs;
        System.Random rand = new System.Random();
        listeJoueursAleatoire = listeJoueurs.OrderBy(joueur => rand.Next()).ToList();
    }
    public void afficherReponse(string reponse, Vector2 position, Vector2 dimension)
    {
        GameObject cadreReponse = GameObject.Find("Background-Reponse(Clone)");


        if (cadreReponse == null)//v�rifie si une r�ponse est d�ja afficher
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

        foreach (Player joueur in listeJoueursAleatoire)
        {
            listScript.AjouterListe(joueur.answer, Color.white);
         }


        scriptPrincipal.askPlayerToVote();
    }

   



    private void OnDestroy()
    {
        PlayingScript scriptJeu = GetComponentInParent<PlayingScript>();
        if (scriptJeu != null)
        {
            scriptJeu.callScoreBoard();    
        }
    }
}
