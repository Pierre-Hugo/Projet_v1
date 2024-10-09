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
        dropdownLanguage = GetComponent<TMP_Dropdown>(); // récupère le drowdown


        // Trouve l'index correspondant à la langue sauvegardée en bouclant sur toutes les options
        int savedLanguageIndex = 0; 
        for (int i = 0; i < dropdownLanguage.options.Count; i++)
        {
            if (dropdownLanguage.options[i].text == manager.getLanguage())
            {
                savedLanguageIndex = i;  // Si on trouve le texte, on garde l'index
                break;  // On sort de la boucle une fois qu'on a trouvé l'index
            }
        }

       
            dropdownLanguage.value = savedLanguageIndex;  // Définit la valeur du Dropdown pour sélectionner l'option trouvée
        

        //est appellé lorsque la value du dropdown change
        dropdownLanguage.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        // Récupère l'élément sélectionné
        string selectedValue = dropdownLanguage.options[index].text;
        
        manager.setLanguage(selectedValue);


        //il serait probablement plus facile de changer le texte si on redémarre le jeu, peut-etre proposer de redémarré
        //le jeu pour appliquer les changements (ou du moins de le faire revenir au menu principale si il est dans une partie
    }

   
}
