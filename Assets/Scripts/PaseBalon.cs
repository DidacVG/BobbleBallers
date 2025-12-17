using UnityEngine;
using UnityEngine.InputSystem;

public class PaseBalon : MonoBehaviour
{
    [Header("Ball")]
    public Rigidbody ballPrefab;
    public Transform spawnPoint;

    [Header("Force Settings")]
    public float maxForce = 8f;
    public float launchMultiplier = 5f;
    public float maxVelocity = 15f;
    public float minDragThreshold = 0.2f;
    public float releaseThreshold = 0.25f;

    private bool dragging = false;
    private float storedMagnitude = 0f;
    private Vector2 storedDirection;

    private Gamepad pad;
    private Tiro tiro;
    private MoverPersonajes mover;

    void Awake()
    {
        tiro = GetComponent<Tiro>();
        mover = GetComponent<MoverPersonajes>();
    }

    // 🔑 ESTE MÉTODO SE LLAMA DESDE PlayerManager
    public void SetPad(Gamepad assignedPad)
    {
        pad = assignedPad;
    }

    void Update()
    {
        if (pad == null) return;
        if (!tiro.HasTheBall) return;
        if (!mover) return;

        // 🔒 SOLO EL JUGADOR ACTIVO DEL EQUIPO
        if (!mover.enabled) return;

        Vector2 stick = pad.rightStick.ReadValue();

        if (!dragging && stick.magnitude > minDragThreshold)
        {
            dragging = true;
            storedMagnitude = 0;
            storedDirection = Vector2.zero;
        }

        if (dragging)
        {
            if (stick.magnitude < releaseThreshold)
            {
                ExecutePass();
                dragging = false;
                return;
            }

            if (stick.magnitude > storedMagnitude)
            {
                storedMagnitude = stick.magnitude;
                storedDirection = stick.normalized;
            }
        }
    }

    void ExecutePass()
    {
        if (storedMagnitude < minDragThreshold) return;

        // 🔒 quitar posesión SOLO aquí
        tiro.HasTheBall = false;

        Rigidbody ballClone = Instantiate(
            ballPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        ballClone.tag = "Bola";

        Vector3 dir = new Vector3(storedDirection.x, 0, storedDirection.y);
        float strength = Mathf.Clamp(storedMagnitude * maxForce, 0, maxForce);

        ballClone.AddForce(dir * strength * launchMultiplier, ForceMode.Impulse);

        if (ballClone.linearVelocity.magnitude > maxVelocity)
            ballClone.linearVelocity =
                ballClone.linearVelocity.normalized * maxVelocity;

        Debug.Log($"{name} PASA el balón");
    }
}
