using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StarScreen : MonoBehaviour
{
    public GameObject playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(playButton);
    }
    public void PressStart()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
