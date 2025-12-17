using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int scoreTeamA = 0;
    public int scoreTeamB = 0;

    [Header("Estado del balón")]
    public MoverPersonajes ballHolder;

    private bool scoringLocked = false;

    [Header("Posesión")]
    public int possessionTeam = 0; // 0 = Team A, 1 = Team B

    [Header("Respawns")]
    public Transform ballSpawnPoint;
    public Transform[] teamASpawnPoints;
    public Transform[] teamBSpawnPoints;

    [Header("Ball")]
    public GameObject currentBall;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ============================
    // ANOTACIÓN
    // ============================
    public void OnScore(MoverPersonajes scorer, int points)
    {
        if (scoringLocked) return;
        scoringLocked = true;

        if (scorer == null) return;

        if (scorer.team == 0)
            scoreTeamA += points;
        else
            scoreTeamB += points;

        // 🔥 mantiene posesión quien anota (regla 3v3)
        possessionTeam = scorer.team;

        ScoreManager.Instance.RefreshUI();

        // ❌ eliminar balón físico
        DestroyCurrentBall();

        StartCoroutine(ResetAfterBasket());
    }

    void DestroyCurrentBall()
    {
        if (currentBall != null)
            Destroy(currentBall);

        currentBall = null;

        if (ballHolder != null)
            ballHolder.HasTheBall = false;

        ballHolder = null;
    }

    void GiveBallToTeam(int team)
    {
        MoverPersonajes[] players =
            (team == 0) ? PlayerManager.Instance.teamA : PlayerManager.Instance.teamB;

        // 🧠 puedes mejorar esto luego (más cercano al balón, etc.)
        MoverPersonajes receiver = players[0];

        GiveBallTo(receiver);
    }

    void ResetPlayersPositions()
{
    for (int i = 0; i < teamASpawnPoints.Length; i++)
    {
        PlayerManager.Instance.teamA[i].transform.position =
            teamASpawnPoints[i].position;
    }

    for (int i = 0; i < teamBSpawnPoints.Length; i++)
    {
        PlayerManager.Instance.teamB[i].transform.position =
            teamBSpawnPoints[i].position;
    }
}


    IEnumerator ResetAfterBasket()
    {
        yield return new WaitForSeconds(1f);

        ResetPlayersPositions();
        GiveBallToTeam(possessionTeam);

        scoringLocked = false;
    }

    public void OnBallOut()
    {
        if (scoringLocked) return;

        scoringLocked = true;

        // 🔁 cambia posesión
        possessionTeam = (possessionTeam == 0) ? 1 : 0;

        DestroyCurrentBall();

        StartCoroutine(ResetAfterOut());
    }

    IEnumerator ResetAfterOut()
    {
        yield return new WaitForSeconds(0.5f);

        ResetPlayersPositions();
        GiveBallToTeam(possessionTeam);

        scoringLocked = false;
    }


    // ============================
    // DAR BALÓN (posesión oficial)
    // ============================
    public void GiveBallTo(MoverPersonajes player)
    {
        if (ballHolder != null)
            ballHolder.HasTheBall = false;

        ballHolder = player;
        player.HasTheBall = true;

        Debug.Log($"BALÓN PARA: {player.name} (Equipo {player.team})");
    }

    // ============================
    // 🚨 PETICIÓN DE PASE (CLAVE)
    // ============================
    public bool CanPass(int teamRequesting)
    {
        if (ballHolder == null) return false;

        // 🔒 SOLO el equipo que tiene el balón puede pasar
        return ballHolder.team == teamRequesting;
    }

    public bool TryPass(MoverPersonajes passer, MoverPersonajes receiver)
    {
        // 🔒 Validaciones críticas
        if (passer == null || receiver == null) return false;
        if (ballHolder != passer) return false;          // 👈 clave
        if (!passer.HasTheBall) return false;
        if (passer.team != receiver.team) return false;  // no pasar al rival

        // Transferir posesión
        passer.HasTheBall = false;
        receiver.HasTheBall = true;
        ballHolder = receiver;

        Debug.Log($"PASE: {passer.name} → {receiver.name}");
        return true;
    }
}
