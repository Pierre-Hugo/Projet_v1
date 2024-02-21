using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScDraw1Script : BaseScenarioPicture
{
    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= tempsAttente)
        {
            if (!questionAsk)
            {
                scriptPrincipal.askPlayerToAnswer();
                questionAsk = true;
                tempsAttente = 15f;
                timer = 0f;
            }
            else
            {
                if (Question != null)
                {
                    Destroy(Question);
                }

                if (listeJoueurs.Count == playerShow) //affiche tout les r�ponse et demande au joueurs de voter pour une r�ponse
                {
                    afficherReponses();
                    tempsAttente = 15f;
                }
                else if (listeJoueurs.Count <= playerShow) //ajoute les points et met fin au sc�nario
                {
                    Destroy(this);
                }
                else //affiche la r�ponse d'un joueur
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
                            afficherReponse(joueur.answer, new Vector2(0f, 360f), new Vector2(1200f, 80f)); //modifier l'emplacement des r�ponses
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


                    tempsAttente = 7f;

                }

                playerShow++;
            }
        }

    }

}
