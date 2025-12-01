using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public MoverPersonajes[] players;
    public int currentIndex = 0;

    private Gamepad pad;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pad = Gamepad.current;

        // Activar solo el primero
        UpdateActivePlayer();
    }

    void Update()
    {
        if (pad == null) pad = Gamepad.current;
        if (pad == null) return;

        // Siguiente
        if (pad.rightShoulder.wasPressedThisFrame)
        {
            currentIndex++;
            if (currentIndex >= players.Length)
                currentIndex = 0;

            UpdateActivePlayer();
        }

        // Anterior
        if (pad.leftShoulder.wasPressedThisFrame)
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = players.Length - 1;

            UpdateActivePlayer();
        }
    }

    void UpdateActivePlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActivePlayer(i == currentIndex);
        }
    }
}
