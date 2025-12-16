using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;

    [Header("Estado del balón")]
    public MoverPersonajes ballHolder;   // ✅ FALTABA ESTO

    private bool scoringLocked = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnScore(MoverPersonajes scorer, int points, bool resetBall = true)
    {
        if (scorer == null) return;

        if (scorer.team == 0)
            scoreTeamA += points;
        else
            scoreTeamB += points;

        Debug.Log($"Marcador → A: {scoreTeamA} | B: {scoreTeamB}");

        ScoreManager.Instance.RefreshUI();

        if (resetBall)
            StartCoroutine(ResetAfterScore());
    }

    IEnumerator ResetAfterScore()
    {
        yield return new WaitForSeconds(1f);
        scoringLocked = false;
    }

    // ✅ MÉTODO OFICIAL PARA DAR EL BALÓN
    public void GiveBallTo(MoverPersonajes player)
    {
        if (ballHolder != null)
            ballHolder.HasTheBall = false;

        ballHolder = player;
        player.HasTheBall = true;

        Debug.Log($"BALÓN PARA: {player.name}");
    }
}
