using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Liste : MonoBehaviour
{

    private GameObject templateCree;
    public GameObject template;
    public float width;
    

    public void AjouterListe(string contenu, Color couleur)
    {
        templateCree = Instantiate(template, transform);
        templateCree.transform.GetChild(0).GetComponent<Text>().text = contenu;
        templateCree.transform.GetChild(0).GetComponent<Text>().color = couleur;
        templateCree.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width, templateCree.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);

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
