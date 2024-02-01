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
    

    void Start()
    {
        Transform parent = transform.parent;
        Transform parentOfParent = parent.parent;
        scriptPrincipal = parentOfParent.GetComponent<ContentManager>();
        questionAsk = false;
        playerShow = 0;

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

                if(listeJoueurs.Count == playerShow) 
                {
                    //show all anwser
                    tempsAttente = 10f;
                }
                else if(listeJoueurs.Count <= playerShow)
                {
                    //ajouter les points selon les votes
                    Destroy(this);
                }
                else
                {
                    //show next answer
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

    private void OnDestroy()
    {
        PlayingScript scriptJeu = GetComponentInParent<PlayingScript>();
        if (scriptJeu != null)
        {
            //scriptJeu.callNewScenario();        Enlever le commentaires SEULEMENT quand les scénarios ne vont pas être hardcode
        }
    }
}
