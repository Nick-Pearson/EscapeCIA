using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuContainer;
    public GameObject BackButton;
    
    public GameObject LevelsContainer;
    public GameObject CreditsContainer;

    public void ShowLevels()
    {
        MainMenuContainer.SetActive(false);
        BackButton.SetActive(true);
        LevelsContainer.SetActive(true);
    }

    public void ShowCredits()
    {
        MainMenuContainer.SetActive(false);
        BackButton.SetActive(true);
        CreditsContainer.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        MainMenuContainer.SetActive(true);
        BackButton.SetActive(false);
        LevelsContainer.SetActive(false);
        CreditsContainer.SetActive(false);
    }
}
