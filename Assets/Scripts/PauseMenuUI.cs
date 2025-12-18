using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void ResumeGame()
    {
        PauseManager.Instance.TogglePause();
    }

    public void RestartMatch()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego"); // para el editor
    }
}

