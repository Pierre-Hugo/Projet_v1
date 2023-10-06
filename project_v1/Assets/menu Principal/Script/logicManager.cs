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

    }
    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void askCreateRoom()
    {

    }

    public void setVolume(float volume)
    {
        volumeMusique.SetFloat("Volume", volume);
    }


    public void closeGame()
    {
        Application.Quit();
    }


}

