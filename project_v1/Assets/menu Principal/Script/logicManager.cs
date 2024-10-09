using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;



public class LogicManager : MonoBehaviour
{
    public AudioMixer volumeMusique;
    
    private void Start()
    {
        initializeOptions();
    }

    private void initializeOptions()
    {
        //s'assurer d'initialiser tout les parametre du menu d'option pour etre sur qu'il soit appliquer au lancement
        volumeMusique.SetFloat("Music", PlayerPrefs.GetFloat("Music", 0.6f));
    }

    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void setVolume( float volume)
    {
        volumeMusique.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", volume);
    }

    public void setLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);
    }
    public string getLanguage()
    {
        string language = PlayerPrefs.GetString("Language");

        if (language == "") return "English"; // l'anglais devient la langue par d�faut
        return language;
    }

    public string getText(int ligne)
    {
        TextAsset ContenuFichier = Resources.Load<TextAsset>("TextFiles/" + getLanguage());

        if (ContenuFichier != null)
        {
            // Utiliser Split pour s�parer le texte par les sauts de ligne
            string[] lines = ContenuFichier.text.Split('\n');

            // V�rifier s'il y a au moins une ligne
            if (lines.Length > 0)
            {
                // Retourner la ligne demand�
                return lines[ligne-1].Trim();  // Trim() pour supprimer les espaces ou sauts de ligne inutiles
            }
          
        }
        
        //devrais pas se rendre la normalement si le fichier existe
        return null;
    }

    public void hideGameObject(GameObject objet)
    {
        objet.SetActive(false);
    }

    public void showGameObject(GameObject objet)
    {
        objet.SetActive(true);
    }

    public void closeGame()
    {
        Application.Quit();
    }

}

