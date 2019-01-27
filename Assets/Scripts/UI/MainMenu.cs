using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuContainer;
    public GameObject BackButton;
    
    public GameObject LevelsContainer;
    public GameObject CreditsContainer;
    public GameObject SavePromptContainer;
    public Text PercentCompleted;

    void Awake()
    {
        PercentCompleted.text = string.Format("Completion {0:0.0}%", FindObjectOfType<GameDataManager>().GetCompletion() * 100.0f);
    }

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

    public void ShowClearSavePrompt()
    {
        SavePromptContainer.SetActive(true);
    }
    public void HideClearSavePrompt()
    {
        SavePromptContainer.SetActive(false);
    }

    public void ClearSave()
    {
        HideClearSavePrompt();
        FindObjectOfType<GameDataManager>().DeleteData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
