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
        // Si hay mandos conectados, asigna los primeros automáticamente
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

        // Avanzar jugador
        if (pad.rightShoulder.wasPressedThisFrame)
        {
            index++;
            if (index >= team.Length) index = 0;
            UpdateActivePlayers();
        }

        // Retroceder jugador
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

            // Activar movimiento
            teamA[i].SetActivePlayer(isActive, teamAPad);

            // Activar tiro (solo si el personaje tiene el componente)
            var tiroA = teamA[i].GetComponent<Tiro>();
            if (tiroA != null)
                tiroA.SetActivePlayer(isActive, teamAPad);
        }

        // --- EQUIPO B ---
        for (int i = 0; i < teamB.Length; i++)
        {
            bool isActive = (i == indexB);

            // Activar movimiento
            teamB[i].SetActivePlayer(isActive, teamBPad);

            // Activar tiro (solo si el personaje tiene el componente)
            var tiroB = teamB[i].GetComponent<Tiro>();
            if (tiroB != null)
                tiroB.SetActivePlayer(isActive, teamBPad);
        }
    }
}
