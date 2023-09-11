using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject optionMenu;
    public GameObject optionBouton;
    private Animator option;

    private void Start()
    {
        option = optionBouton.GetComponent<Animator>();
    }
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
        
        
        option.SetBool("IsMenuActif", false);
        option.Play("BaseOption");
        MainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    public void goToMenu()
    {
        optionMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}
