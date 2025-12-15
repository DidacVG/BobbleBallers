using UnityEngine;
using UnityEngine.InputSystem;

public class PaseBalon : MonoBehaviour
{
    [Header("Ball")]
    public Rigidbody ballPrefab;          // Prefab del balón a instanciar
    public Transform spawnPoint;          // El punto desde donde sale el balón

    [Header("Force Settings")]
    public float maxForce = 0f;
    public float launchMultiplier = 5f;
    public float maxVelocity = 15f;
    public float minDragThreshold = 0.2f;
    public float releaseThreshold = 0.25f;

    private bool dragging = false;
    private float storedMagnitude = 0f;
    private Vector2 storedDirection;
    public Tiro posesion;

    void Update()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null) return;

        // Leer stick derecho
        Vector2 stick = pad.rightStick.ReadValue();

        // INICIO DEL ARRASTRE
        if (!dragging && stick.magnitude > minDragThreshold)
        {
            dragging = true;
            storedMagnitude = 0;
            storedDirection = Vector2.zero;
        }

        // ARRASTRE
        if (dragging)
        {
            // Lanzar cuando vuelve a neutro
            if (stick.magnitude < releaseThreshold)
            {
                if (posesion.HasTheBall == true)
                {
                    Launch();
                    dragging = false;
                    posesion.HasTheBall = false;
                }
            }

            // Registrar máximo
            if (stick.magnitude > storedMagnitude)
            {
                storedMagnitude = stick.magnitude;
                storedDirection = stick.normalized;
            }
        }
    }

    // ------------------------------------
    // LANZAMIENTO: INSTANCIA UN BALÓN NUEVO
    // ------------------------------------
    void Launch()
    {
        if (storedMagnitude < minDragThreshold)
            return;

        // Crear nuevo balón
        Rigidbody ballClone = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        ballClone.tag = "Bola";

        // Convertir dirección 2D → 3D
        Vector3 forceDir = new Vector3(storedDirection.x, 0, storedDirection.y);

        // Calcular fuerza
        float strength = Mathf.Clamp(storedMagnitude * maxForce, 0, maxForce);

        // Aplicar impulso
        ballClone.AddForce(forceDir * strength * launchMultiplier, ForceMode.Impulse);

        // Limitar velocidad
        if (ballClone.linearVelocity.magnitude > maxVelocity)
            ballClone.linearVelocity = ballClone.linearVelocity.normalized * maxVelocity;

        Debug.Log("PASADO → fuerza: " + strength + " dirección: " + storedDirection);
    }
}
