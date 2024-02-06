using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScenarioWord : MonoBehaviour
{
    public Text textRceu;
    protected List<Player> listeJoueurs;
    protected ContentManager scriptPrincipal;
    protected float timer;
    protected float tempsAttente;
    protected bool questionAsk;
    protected int playerShow;
    protected List<Player> listeJoueursDejaAfficher;
    public GameObject modeleReponse;
    public GameObject Question;

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

    

    public void initialisation(string motPlaque, List<Player> Joueurs)
    {
        textRceu.text = motPlaque;
        listeJoueurs = Joueurs;
    }
    public void afficherReponse(string reponse, Vector2 position, Vector2 dimension)
    {
        GameObject cadreReponse = GameObject.Find("Background-Reponse(Clone)");




        if (cadreReponse == null)//vérifie si une réponse est déja afficher
        {
            cadreReponse = Instantiate(modeleReponse, transform);
            cadreReponse.GetComponent<RectTransform>().anchoredPosition = position;
            cadreReponse.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500f, cadreReponse.GetComponent<RectTransform>().sizeDelta.y);
        }


        cadreReponse.transform.GetChild(0).GetComponent<Text>().text = reponse;
    }

    protected void afficherReponses() 
    {
        GameObject cadreReponse = GameObject.Find("Background-Reponse(Clone)");
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
