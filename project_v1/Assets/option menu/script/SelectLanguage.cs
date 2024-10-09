using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectLanguage : MonoBehaviour
{
    private TMP_Dropdown dropdownLanguage;
    public LogicManager manager;
    // Start is called before the first frame update
    void Start()
    {
        dropdownLanguage = GetComponent<TMP_Dropdown>(); // r�cup�re le drowdown


        // Trouve l'index correspondant � la langue sauvegard�e en bouclant sur toutes les options
        int savedLanguageIndex = 0; 
        for (int i = 0; i < dropdownLanguage.options.Count; i++)
        {
            if (dropdownLanguage.options[i].text == manager.getLanguage())
            {
                savedLanguageIndex = i;  // Si on trouve le texte, on garde l'index
                break;  // On sort de la boucle une fois qu'on a trouv� l'index
            }
        }

       
            dropdownLanguage.value = savedLanguageIndex;  // D�finit la valeur du Dropdown pour s�lectionner l'option trouv�e
        

        //est appell� lorsque la value du dropdown change
        dropdownLanguage.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        // R�cup�re l'�l�ment s�lectionn�
        string selectedValue = dropdownLanguage.options[index].text;
        
        manager.setLanguage(selectedValue);


        //il serait probablement plus facile de changer le texte si on red�marre le jeu, peut-etre proposer de red�marr�
        //le jeu pour appliquer les changements (ou du moins de le faire revenir au menu principale si il est dans une partie
    }

   
}
