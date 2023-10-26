using System;

public class Player
{
    public string Id { get; set; }
    public string Pseudo { get; set; }
    public int Points { get; private set; }
    public string Couleur { get; set; }

    public Player(string id, string pseudo)
    {
        Id = id;
        Pseudo = pseudo;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }
}
