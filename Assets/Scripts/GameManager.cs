using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;

    public Transform ballRespawnPoint;
    public MoverPersonajes lastScorer;

    private bool scoringLocked = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnScore(MoverPersonajes scorer, int points, bool resetBall = true)
    {
        if (scoringLocked) return;
        scoringLocked = true;

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

    IEnumerator PerfectShotSlowMotion()
    {
        Time.timeScale = 0.4f;
        yield return new WaitForSecondsRealtime(0.6f);
        Time.timeScale = 1f;
    }

    public void GiveBallTo(MoverPersonajes player)
    {
        player.HasTheBall = true;
    }
}
