using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Liste : MonoBehaviour
{

    private GameObject templateCree;
    public GameObject template;
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AjouterListe(string contenu, Color couleur)
    {
        templateCree = Instantiate(template, transform);
        templateCree.transform.GetChild(0).GetComponent<Text>().text = contenu;
        templateCree.transform.GetChild(0).GetComponent<Text>().color = couleur;

    }

    public void retirerListe(string contenu)
    {
        foreach (Transform cadreTransform in transform)
        {
            GameObject cadre = cadreTransform.gameObject;

            if (cadre.transform.GetChild(0).GetComponent<Text>().text == contenu)
            {
                Destroy(cadre);
                break;
            }
        }
    }

    public void ChangerCouleur(string Text, Color couleur)
    {
        foreach (Transform cadreTransform in transform)
        {
            GameObject cadre = cadreTransform.gameObject;

            if (cadre.transform.GetChild(0).GetComponent<Text>().text == Text)
            {
                cadre.transform.GetChild(0).GetComponent<Text>().color =couleur;
                break;
            }
        }
    }
}
