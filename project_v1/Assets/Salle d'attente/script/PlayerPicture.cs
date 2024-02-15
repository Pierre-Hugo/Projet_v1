
using System;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class PlayerPicture : Player
{
   
    public bool isDraw { get; set; }

    public string img { get; set; }

    public PlayerPicture(string id, string pseudo, Color couleur, string image, bool isDraw) : base(id, pseudo, couleur)
    {
        this.isDraw = isDraw;
        this.img = image;
        

               
        
    }

    

    
}