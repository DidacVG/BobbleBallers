using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Equipos")]
    public MoverPersonajes[] teamA;
    public MoverPersonajes[] teamB;

    private int indexA = 0;
    private int indexB = 0;

    [Header("Mandos asignados a cada equipo")]
    public Gamepad teamAPad;
    public Gamepad teamBPad;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (Gamepad.all.Count > 0)
            teamAPad = Gamepad.all[0];

        if (Gamepad.all.Count > 1)
            teamBPad = Gamepad.all[1];

        UpdateActivePlayers();
    }

    void Update()
    {
        HandleTeamInput(teamAPad, teamA, ref indexA);
        HandleTeamInput(teamBPad, teamB, ref indexB);
    }

    void HandleTeamInput(Gamepad pad, MoverPersonajes[] team, ref int index)
    {
        if (pad == null) return;
        if (team.Length == 0) return;

        if (pad.rightShoulder.wasPressedThisFrame)
        {
            index++;
            if (index >= team.Length) index = 0;
            UpdateActivePlayers();
        }

        if (pad.leftShoulder.wasPressedThisFrame)
        {
            index--;
            if (index < 0) index = team.Length - 1;
            UpdateActivePlayers();
        }
    }

    void UpdateActivePlayers()
    {
        // --- EQUIPO A ---
        for (int i = 0; i < teamA.Length; i++)
        {
            bool isActive = (i == indexA);

            teamA[i].SetActivePlayer(isActive, teamAPad);

            var tiro = teamA[i].GetComponent<Tiro>();
            if (tiro != null)
            {
                tiro.SetActivePlayer(isActive, teamAPad);
                tiro.SetPad(teamAPad);   // ← AÑADIDO AQUÍ
            }
        }

        // --- EQUIPO B ---
        for (int i = 0; i < teamB.Length; i++)
        {
            bool isActive = (i == indexB);

            teamB[i].SetActivePlayer(isActive, teamBPad);

            var tiro = teamB[i].GetComponent<Tiro>();
            if (tiro != null)
            {
                tiro.SetActivePlayer(isActive, teamBPad);
                tiro.SetPad(teamBPad);   // ← AÑADIDO AQUÍ
            }
        }
    }

    public MoverPersonajes GetClosestTeammate(MoverPersonajes fromPlayer)
    {
        MoverPersonajes[] team = (fromPlayer.team == 0) ? teamA : teamB;

        float bestDist = Mathf.Infinity;
        MoverPersonajes best = null;

        foreach (var p in team)
        {
            if (p == fromPlayer) continue;

            float d = Vector3.Distance(fromPlayer.transform.position, p.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = p;
            }
        }

        return best;
    }
}