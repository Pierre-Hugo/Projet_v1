using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScoreBoardScipt : MonoBehaviour
{
    public GameObject row;
    public Transform rowParent;
    private List<Player> listeJoueurs;
    private float timeToLive;
    private float compteurs;
    void Start()
    {
        timeToLive = 13f;
        compteurs = 0f;
        ContentManager contentManager = FindObjectOfType<ContentManager>();
        listeJoueurs = contentManager.getListeJoueurs();
        listeJoueurs = listeJoueurs.OrderByDescending(joueur => joueur.Points).ToList();
        int position = 1;
        foreach (Player player in listeJoueurs)
        {
            GameObject newRow  = Instantiate(row,rowParent);
            Text[] texts = newRow.GetComponentsInChildren<Text>();
            texts[0].text = position.ToString();
            texts[1].text = player.Pseudo;
            texts[2].text = player.Points.ToString();

            position++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        compteurs+= Time.deltaTime;
        if (timeToLive < compteurs)
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        PlayingScript scriptJeu = GetComponentInParent<PlayingScript>();
        if (scriptJeu != null)
        {
            scriptJeu.callNewScenario();
        }
    }
}
