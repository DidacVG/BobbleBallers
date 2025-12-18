using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginPractice()
    {
        SceneManager.LoadScene("Practice"); // si tienes otra escena
    }
}
