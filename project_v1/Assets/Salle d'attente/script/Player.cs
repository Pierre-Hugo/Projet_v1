using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string id;
    private string pseudo;
    private int points;

    public Player(string _id, string _pseudo)
    {
        pseudo = _pseudo;
        id = _id;
    }

    public string getId()
    {
        return id;
    }

    public string getPseudo()
    {
        return pseudo;
    }

    public int getPoints()
    {
        return points;
    }

    public void addPoint(int pointAjoute)
    {
        points += pointAjoute;
    }
    public void removePoint(int pointEnleve)
    {
        points -= pointEnleve;
    }

    public void resetPoint()
    {
        points = 0;
    }

}
