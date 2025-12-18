using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("First Selected")]
    public GameObject playButton;
    public GameObject backButton;

    void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(playButton);
    }

    // ==========================
    // BOTONES
    // ==========================

    public void PlayGame()
    {
        SceneManager.LoadScene("Juego"); // nombre exacto
    }

    public void PracticeMode()
    {
        SceneManager.LoadScene("Tutorial"); // si tienes otra escena
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void QuitGame()
    {
        Debug.Log("Cerrando juego...");
        Application.Quit();
    }
}
