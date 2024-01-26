
using System;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class PlayerPicture : Player
{
    public Texture2D imageTexture { get; set; }
    public bool isDraw { get; set; }

    public PlayerPicture(string id, string pseudo, Color couleur, string imageHexa, bool isDraw) : base(id, pseudo, couleur)
    {
        this.isDraw = isDraw;
        

        // Convertir la chaîne hexadécimale en tableau de bytes
        byte[] imageBytes = HexStringToByteArray(imageHexa);

        // Charger la texture à partir des bytes
        imageTexture = ByteArrayToTexture2D(imageBytes);

        // Créer un sprite à partir de la texture
        //imageSprite = Sprite.Create(imageTexture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0.5f, 0.5f));

        
        
    }

    // Convertir une chaîne hexadécimale en tableau de bytes
    static byte[] HexStringToByteArray(string hexString)
    {
        int length = hexString.Length;
        byte[] bytes = new byte[length / 2];

        for (int i = 0; i < length; i += 2)
        {
            bytes[i / 2] = System.Convert.ToByte(hexString.Substring(i, 2), 16);
        }

        return bytes;
    }

    // Convertir un tableau de bytes en Texture2D
    static Texture2D ByteArrayToTexture2D(byte[] byteArray)
    {
        Texture2D texture = new Texture2D(2, 2); // Remplacez 2, 2 par les dimensions de votre image
        texture.LoadImage(byteArray); // Chargez l'image depuis les bytes

        return texture;
    }

    
}