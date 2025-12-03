using UnityEngine;
using UnityEngine.InputSystem;

public class PaseBalon : MonoBehaviour
{
    public bool HasTheBall = false;          // Este jugador tiene la pelota
    public Transform ball;                   // La pelota en escena
    public Rigidbody ballRb;

    public float maxPassForce = 10f;
    public float minDragThreshold = 0.2f;
    public float releaseThreshold = 0.15f;

    public float passMultiplier = 25f;

    private Gamepad pad;
    private bool isActivePlayer = false;

    private bool dragging = false;
    private float storedMagnitude = 0;
    private Vector2 storedDirection = Vector2.zero;

    public LineRenderer lineRenderer;

    void Start()
    {
        pad = Gamepad.current;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (!isActivePlayer) return;
        if (!HasTheBall) return;
        if (pad == null) return;

        Vector2 stick = pad.rightStick.ReadValue();

        // Debug opcional
        // Debug.Log("Stick derecho: " + stick);

        // Inicio de arrastre
        if (!dragging && stick.magnitude > minDragThreshold)
        {
            dragging = true;
            storedMagnitude = 0;
            storedDirection = Vector2.zero;

            if (lineRenderer != null)
                lineRenderer.enabled = true;
        }

        // Arrastre
        if (dragging)
        {
            if (stick.magnitude > storedMagnitude)
            {
                storedMagnitude = stick.magnitude;
                storedDirection = stick.normalized;
            }

            DrawLine(storedDirection, storedMagnitude);

            // Cuando vuelve a neutro → pasar
            if (stick.magnitude < releaseThreshold)
            {
                PassBall();
                dragging = false;
                if (lineRenderer != null) lineRenderer.enabled = false;
            }
        }
    }

    void DrawLine(Vector2 dir, float mag)
    {
        if (lineRenderer == null) return;

        float strength = Mathf.Clamp(mag * maxPassForce, 0, maxPassForce);

        Vector3 start = transform.position + Vector3.up * 0.1f;
        Vector3 end = start + new Vector3(dir.x, 0, dir.y) * (strength * 1.5f);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void PassBall()
    {
        if (!HasTheBall) return;
        if (ball == null || ballRb == null) return;
        if (storedMagnitude < 0.2f) return;

        // Quitar posesión
        HasTheBall = false;

        // Desanclar pelota
        ball.SetParent(null);
        ballRb.isKinematic = false;

        Vector3 passDir = new Vector3(storedDirection.x, 0, storedDirection.y);
        float strength = Mathf.Clamp(storedMagnitude * passMultiplier, 0, passMultiplier);

        ballRb.linearVelocity = passDir * strength;

        Debug.Log("PASE ejecutado → " + passDir + " (fuerza " + strength + ")");
    }

    public void SetActivePlayer(bool state)
    {
        isActivePlayer = state;

        if (!state && lineRenderer != null)
            lineRenderer.enabled = false;
    }
}
