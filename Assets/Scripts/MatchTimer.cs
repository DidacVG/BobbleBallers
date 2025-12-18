using TMPro;
using UnityEngine;

public class MatchTimer : MonoBehaviour
{
    [Header("Tiempo de partido (segundos)")]
    public float matchDuration = 180f; // 3 minutos

    private float remainingTime;
    private bool matchEnded = false;

    public AudioSource audioSource;
    public AudioClip endMatchClip;

    [Header("UI")]
    public TMP_Text timerText;

    void Start()
    {
        remainingTime = matchDuration;
        UpdateUI();
    }

    void Update()
    {
        if (matchEnded) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndMatch();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void EndMatch()
    {
        matchEnded = true;

        Debug.Log("⏱ FIN DEL PARTIDO");

        // 🔊 Reproducir sonido
        if (audioSource != null && endMatchClip != null)
        {
            audioSource.PlayOneShot(endMatchClip);
        }

        CheckWinner();
    }

    void CheckWinner()
    {
        int scoreA = GameManager.Instance.scoreTeamA;
        int scoreB = GameManager.Instance.scoreTeamB;

        if (GameManager.Instance.scoreTeamA > GameManager.Instance.scoreTeamB)
        {
            MatchUI.Instance.ShowResult("BLUE TEAM WINS", Color.cyan);
        }
        else if (GameManager.Instance.scoreTeamB > GameManager.Instance.scoreTeamA)
        {
            MatchUI.Instance.ShowResult("ORANGE TEAM WINS", Color.red);
        }
        else
        {
            MatchUI.Instance.ShowResult("THAT'S A DRAW", Color.yellow);
        }

        // 🔒 Bloquear juego
        GameManager.Instance.enabled = false;
        PlayerManager.Instance.enabled = false;
    }
}
