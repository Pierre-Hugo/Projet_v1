using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BaseScenarioVideo : BaseScenario
{
    public RawImage video;
    float videoDuration;
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
                    timer = 0f;
                }
                else if (listeJoueurs.Count <= playerShow) //ajoute les points et met fin au sc�nario
                {
                    Destroy(gameObject);
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


    public void initialisation(string vid, List<Player> Joueurs)
    {
        base.initialisation(Joueurs);
        LoadVideoFromDataUrl(vid, video); // affiche l'image recu dans le sc�nario

    }


    protected void LoadVideoFromDataUrl(string dataUrl, RawImage rawImage)
    {
        // Divisez l'URL en parties pour extraire le type de m�dia et les donn�es base64
        string[] parts = dataUrl.Split(',');
        string base64Data = parts[1];

        // Convertissez les donn�es base64 en tableau d'octets
        byte[] videoData = System.Convert.FromBase64String(base64Data);

        // Enregistrez la vid�o dans un fichier temporaire (ou utilisez VideoPlayer.url avec Unity 2017.1+)
        string videoPath = Application.persistentDataPath + "/tempVideo.mp4";
        System.IO.File.WriteAllBytes(videoPath, videoData);

        // Chargez la vid�o dans VideoPlayer
        VideoPlayer videoPlayer = rawImage.GetComponent<VideoPlayer>();
        videoPlayer.url = videoPath;


        // Configurez le RawImage pour afficher la sortie du VideoPlayer
        rawImage.texture = videoPlayer.targetTexture;
        timer += (float)videoPlayer.length;
        // Commencez la lecture de la vid�o
        videoPlayer.Play();
    }


}
