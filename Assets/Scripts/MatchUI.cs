using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MatchUI : MonoBehaviour
{
    public static MatchUI Instance;

    public TextMeshProUGUI resultText;

    public GameObject backToMenuButton;

    void Awake()
    {
        Instance = this;
        resultText.gameObject.SetActive(false);
    }

    public void ShowResult(string message, Color color)
    {
        resultText.text = message;
        resultText.color = color;
        resultText.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backToMenuButton);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // 🔥 MUY IMPORTANTE si vienes de pausa
        SceneManager.LoadScene("MainMenu");
    }
}

