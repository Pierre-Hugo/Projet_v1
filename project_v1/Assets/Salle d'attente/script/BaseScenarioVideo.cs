using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BaseScenarioVideo : BaseScenario
{
    public RawImage video;
    float videoDuration;
   

    public void initialisation(string vid, List<Player> Joueurs)
    {
        base.initialisation(Joueurs);
        LoadVideoFromDataUrl(vid, video); // charge la vid�o a partir de l'url recu

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
