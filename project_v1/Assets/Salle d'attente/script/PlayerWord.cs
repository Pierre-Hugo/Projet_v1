using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWord : Player
{
    public string word { get; set; }

    public PlayerWord(string id, string pseudo, Color couleur, string Word) : base(id, pseudo, couleur)
    {
        word = Word;
    }

    
}
