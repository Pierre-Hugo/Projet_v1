using UnityEngine;


public class Player
{
    public string Id { get; set; }
    public string Pseudo { get; set; }
    public string answer { get; set; }
    public int Points { get; private set; }
    public int nbVote { get; set; }
    public Color Couleur { get; set; }

    public Player(string id, string pseudo, Color couleur)
    {
        Id = id;
        Pseudo = pseudo;
        Couleur = couleur;
        Points = 0;
        nbVote = 0;
        answer = string.Empty;

    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void setAnswer(string answer)
    {
        this.answer = answer;
    }
}
