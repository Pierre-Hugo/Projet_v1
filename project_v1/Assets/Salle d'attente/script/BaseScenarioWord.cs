using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;


public class BaseScenarioWord : BaseScenario
{
    public Text textRecu;

    public void initialisation(string mot, List<Player> Joueurs)
    {
        base.initialisation(Joueurs);
        textRecu.text = mot;

    }
}
    
