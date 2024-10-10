using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSetText : MonoBehaviour
{
    private LogicManager manager;
    private Text text;
    public int ligneRecupere;
    public void Start()
    {
        manager = FindObjectOfType<LogicManager>();
        text = GetComponent<Text>();

        text.text= manager.getText(ligneRecupere);

        
    }
}
