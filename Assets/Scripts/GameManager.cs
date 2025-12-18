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

    [Header("Power Ups")]
    private bool doublePointsTeamA = false;
    private bool doublePointsTeamB = false;

    [Header("Power Ups")]
    public GameObject[] powerUpPrefabs;   // Prefabs posibles
    public Transform[] powerUpSpawnPoints; // Puntos válidos del campo

    public int basketsForPowerUp = 3;

    private int basketCounter = 0;
    private GameObject currentPowerUp;

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

        int finalPoints = points;

        // 🟣 DOBLE PUNTUACIÓN
        if (scorer.team == 0 && doublePointsTeamA)
        {
            finalPoints *= 2;
            doublePointsTeamA = false;
            Debug.Log("🔥 x2 consumido por Equipo A");
        }
        else if (scorer.team == 1 && doublePointsTeamB)
        {
            finalPoints *= 2;
            doublePointsTeamB = false;
            Debug.Log("🔥 x2 consumido por Equipo B");
        }

        if (scorer.team == 0)
            scoreTeamA += finalPoints;
        else
            scoreTeamB += finalPoints;

        possessionTeam = scorer.team;

        ScoreManager.Instance.RefreshUI();

        basketCounter++;

        if (basketCounter >= basketsForPowerUp)
        {
            basketCounter = 0;
            TrySpawnPowerUp();
        }

        StartCoroutine(ResetAfterBasket());
    }

    void TrySpawnPowerUp()
    {
        if (currentPowerUp != null)
            return; // Ya hay uno activo

        if (powerUpPrefabs.Length == 0 || powerUpSpawnPoints.Length == 0)
            return;

        GameObject prefab =
            powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        Transform spawnPoint =
            powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];

        currentPowerUp = Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        Debug.Log("🎁 Power-Up generado");
    }

    public void NotifyPowerUpConsumed()
    {
        currentPowerUp = null;
    }

    bool IsSpawnSafe(Vector3 pos)
    {
        foreach (var p in PlayerManager.Instance.teamA)
            if (Vector3.Distance(p.transform.position, pos) < 1.5f)
                return false;

        foreach (var p in PlayerManager.Instance.teamB)
            if (Vector3.Distance(p.transform.position, pos) < 1.5f)
                return false;

        return true;
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

    public void StartInvertControls(int team, float duration)
    {
        StartCoroutine(InvertControlsCoroutine(team, duration));
    }

    IEnumerator InvertControlsCoroutine(int team, float duration)
    {
        Debug.Log($"🔀 Controles invertidos para equipo {team}");

        MoverPersonajes[] targets =
            (team == 0) ? PlayerManager.Instance.teamA : PlayerManager.Instance.teamB;

        foreach (var p in targets)
            p.SetInvertControls(true);

        yield return new WaitForSeconds(duration);

        foreach (var p in targets)
            p.SetInvertControls(false);

        Debug.Log($"✅ Controles restaurados para equipo {team}");
    }

    public void ActivateDoublePoints(int team)
    {
        if (team == 0)
        {
            doublePointsTeamA = true;
            Debug.Log("🟣 Equipo A ACTIVÓ x2 puntos");
        }
        else
        {
            doublePointsTeamB = true;
            Debug.Log("🟣 Equipo B ACTIVÓ x2 puntos");
        }
    }
}
