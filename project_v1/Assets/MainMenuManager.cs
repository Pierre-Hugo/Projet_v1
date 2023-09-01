using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject optionMenu;
    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void closeGame()
    {
        Application.Quit();
    }

    public void goToOption()
    {
        MainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    public void goToMenu()
    {
        optionMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}
