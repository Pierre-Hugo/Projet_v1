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

