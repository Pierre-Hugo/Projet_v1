using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScWord1Script : MonoBehaviour
{
    public Text textPlaque;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initialisation(string motPlaque) 
    {
        textPlaque.text = motPlaque;
    }

    private void OnDestroy()
    {
        //call new scénario
    }
}
