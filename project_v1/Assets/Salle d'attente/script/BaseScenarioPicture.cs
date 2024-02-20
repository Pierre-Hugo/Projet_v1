using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScenarioPicture : BaseScenario
{

    protected string dataUrlRecu;
    public Image affiche;

    public void initialisation(string img, List<Player> Joueurs)
    {
        base.initialisation(Joueurs);
        LoadSpriteFromDataUrl(img, affiche); // affiche l'image recu dans le sc�nario

    }

    void LoadSpriteFromDataUrl(string dataUrl, Image image)
    {
        // Divisez l'URL en parties pour extraire le type de m�dia et les donn�es base64
        string[] parts = dataUrl.Split(',');
        string mediaType = parts[0].Split(':')[1].Split(';')[0];
        string base64Data = parts[1];

        // Convertissez les donn�es base64 en tableau d'octets
        byte[] imageData = System.Convert.FromBase64String(base64Data);

        // Cr�ez une texture � partir des octets
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);

        // Cr�ez un sprite � partir de la texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Utilisez le sprite comme vous le souhaitez, par exemple, affectez-le � un objet SpriteRenderer
        image.sprite = sprite;
    }
}
