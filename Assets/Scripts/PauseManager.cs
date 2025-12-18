using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Button firstSelectedButton;

    public static PauseManager Instance;

    public GameObject pausePanel;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        foreach (var pad in Gamepad.all)
        {
            if (pad.startButton.wasPressedThisFrame)
            {
                TogglePause();
                break;
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;

        TogglePlayerMovement(!isPaused);

        if (isPaused)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
        }
    }

    // 🔥 ACTIVAR / DESACTIVAR MOVIMIENTO
    void TogglePlayerMovement(bool enable)
    {
        if (PlayerManager.Instance == null) return;

        foreach (var p in PlayerManager.Instance.teamA)
            if (p != null) p.enabled = enable;

        foreach (var p in PlayerManager.Instance.teamB)
            if (p != null) p.enabled = enable;
    }

    // Para botón UI
    public void Resume()
    {
        if (isPaused)
            TogglePause();
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}


