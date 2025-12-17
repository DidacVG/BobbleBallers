using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int scoreTeamA;
    public int scoreTeamB;

    public MoverPersonajes ballHolder;

    public int possessionTeam = 0; // 0 = A, 1 = B
    private bool scoringLocked = false;

    [Header("Ball")]
    public Rigidbody ballPrefab;
    private GameObject currentBall;

    [Header("Respawns")]
    public Transform[] teamASpawnPoints;
    public Transform[] teamBSpawnPoints;

    [Header("Ball Spawn Points")]
    public Transform ballSpawnTeamA;
    public Transform ballSpawnTeamB;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // =============================
    // 🏀 CANASTA
    // =============================
    public void OnScore(MoverPersonajes scorer, int points)
    {
        if (scoringLocked) return;
        scoringLocked = true;

        if (scorer == null) return;

        if (scorer.team == 0)
            scoreTeamA += points;
        else
            scoreTeamB += points;


        // 🔥 EN 3v3 EL QUE ANOTA MANTIENE POSESIÓN
        possessionTeam = scorer.team;

        ScoreManager.Instance.RefreshUI();

        StartCoroutine(ResetAfterBasket());
    }

    IEnumerator ResetAfterBasket()
    {
        yield return new WaitForSeconds(1f);

        ResetPlayersPositions();
        SpawnBallForTeam(possessionTeam);

        scoringLocked = false;
    }

    // =============================
    // 🚨 BALÓN FUERA
    // =============================
    public void OnBallOut()
    {
        int lastTeam = ballHolder != null ? ballHolder.team : possessionTeam;

        possessionTeam = (lastTeam == 0) ? 1 : 0;

        Debug.Log($"🚨 BALÓN FUERA → posesión para Equipo {possessionTeam}");

        StartCoroutine(ResetAfterOut());
    }

    IEnumerator ResetAfterOut()
    {
        yield return new WaitForSeconds(0.5f);

        ResetPlayersPositions();
        SpawnBallForTeam(possessionTeam);
    }

    // =============================
    // 🔄 RESETS
    // =============================
    void ResetPlayersPositions()
    {
        for (int i = 0; i < teamASpawnPoints.Length; i++)
            PlayerManager.Instance.teamA[i].transform.position =
                teamASpawnPoints[i].position;

        for (int i = 0; i < teamBSpawnPoints.Length; i++)
            PlayerManager.Instance.teamB[i].transform.position =
                teamBSpawnPoints[i].position;
    }

    void GiveBallToTeam(int team)
    {
        MoverPersonajes[] players =
            (team == 0) ? PlayerManager.Instance.teamA : PlayerManager.Instance.teamB;

        GiveBallTo(players[0]); // simple, estable
    }

    public void GiveBallTo(MoverPersonajes player)
    {
        if (ballHolder != null)
            ballHolder.HasTheBall = false;

        ballHolder = player;
        player.HasTheBall = true;

        Debug.Log($"BALÓN PARA {player.name} (Equipo {player.team})");
    }

    public void SpawnBallForTeam(int team)
    {
        ClearBall();

        GameObject oldBall = GameObject.FindGameObjectWithTag("Bola");
        if (oldBall != null)
            Destroy(oldBall);

        Transform spawn = (team == 0) ? ballSpawnTeamA : ballSpawnTeamB;

        Rigidbody newBall = Instantiate(
            ballPrefab,
            spawn.position,
            spawn.rotation
        );

        newBall.tag = "Bola";

        MoverPersonajes[] players =
            (team == 0) ? PlayerManager.Instance.teamA : PlayerManager.Instance.teamB;

        GiveBallTo(players[0]);

        Debug.Log($"🏀 SAQUE PARA EQUIPO {team}");
    }

    public void ClearBall()
    {
        if (ballHolder != null)
        {
            ballHolder.HasTheBall = false;
            ballHolder = null;
        }
    }
}
