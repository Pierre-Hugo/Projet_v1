using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVideo : Player
{
    // variable pour stocker la video
    public string vid { get; set; }
    public PlayerVideo(string id, string pseudo, Color couleur, string Video) : base(id, pseudo, couleur)
    {
        this.vid = Video;
    }
}
