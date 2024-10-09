using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSetText : MonoBehaviour
{
    public LogicManager manager;
    private Text text;
    public int ligneRecupere;
    public void Start()
    {
        text = GetComponent<Text>();

        text.text= manager.getText(ligneRecupere);

        
    }
}
