using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScWord1Script : BaseScenarioWord
{
   


    

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
            else
            {
                if (Question !=null)  
                {
                Destroy(Question);
                }

                if(listeJoueurs.Count == playerShow) //affiche tout les réponse et demande au joueurs de voter pour une réponse
                {
                    afficherReponses();
                    tempsAttente = 10f;
                }
                else if(listeJoueurs.Count <= playerShow) //ajoute les points et met fin au scénario
                {
                    givePoints();
                    Destroy(this);
                }
                else //affiche la réponse d'un joueur
                {
                    timer = 0f;
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
                                    afficherReponse(joueur.answer, new Vector2(0f, 360f), new Vector2(700f,80f));
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

    
    

   
}
