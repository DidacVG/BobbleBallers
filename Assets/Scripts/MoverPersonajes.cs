using UnityEngine;
using UnityEngine.InputSystem;

public class MoverPersonajes : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;

    public float launchMultiplier = 5f;
    public float maxForce = 300f;
    public float minVelocityToMove = 0.1f;
    public float rotationSpeed = 10f;
    public float maxTilt = 30f;

    public Rigidbody rb;
    public LineRenderer lineRenderer;
    private bool isActivePlayer = false;

    private bool dragging = false;
    private Vector2 stickStart;
    private Vector2 currentStick;

    private Gamepad pad;
    private bool vibrating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        pad = Gamepad.current;
    }

    void Update()
    {
        if (pad == null || !isActivePlayer) return;

        Vector3 currentVel = rb.IsSleeping() ? Vector3.zero : rb.GetPointVelocity(rb.worldCenterOfMass);

        // ---------------- ROTACIÓN EN MOVIMIENTO ----------------
        if (currentVel.sqrMagnitude > minVelocityToMove * minVelocityToMove)
        {
            Vector3 hVel = new Vector3(currentVel.x, 0, currentVel.z);
            Quaternion targetRot = Quaternion.LookRotation(hVel);

            float speed = hVel.magnitude;
            float tilt = Mathf.Lerp(0, maxTilt, speed / 10f);
            Quaternion tiltRot = Quaternion.Euler(tilt, 0, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot * tiltRot, Time.deltaTime * rotationSpeed);

            dragging = false;
            lineRenderer.enabled = false;
            StopVibration();
            return;
        }

        // ---------------- INPUT STICK ----------------
        Vector2 stick = pad.leftStick.ReadValue();

        // Inicio de arrastre
        if (!dragging && stick.magnitude > 0.2f)
        {
            dragging = true;
            lineRenderer.enabled = true;
            StartVibration(0.1f);
        }

        if (dragging)
        {
            // Línea usando el stick directamente
            DrawLine(stick);

            float intensity = Mathf.Clamp(stick.magnitude * 1.5f, 0f, 1f);
            StartVibration(intensity);

            // Lanzar cuando vuelve a neutro
            if (stick.magnitude < 0.15f)
            {
                dragging = false;
                lineRenderer.enabled = false;
                StopVibration();
                LaunchOpposite(stick);
            }
        }
        else
        {
            StopVibration();
        }
    }

    // --------------------------
    // DIBUJAR LÍNEA CONSISTENTE
    // --------------------------
    void DrawLine(Vector2 stick)
    {
        float strength = Mathf.Clamp(stick.magnitude * maxForce, 0, maxForce);
        Vector2 opposite = -stick.normalized;

        Vector3 start = rb.transform.position;
        Vector3 end = start + new Vector3(opposite.x, 0, opposite.y) * (strength / 40f);

        lineRenderer.SetPosition(0, start + Vector3.up * 0.1f);
        lineRenderer.SetPosition(1, end + Vector3.up * 0.1f);
    }

    // --------------------------
    // LANZAMIENTO CONSISTENTE
    // --------------------------
    void LaunchOpposite(Vector2 stick)
    {
        float strength = Mathf.Clamp(stick.magnitude * maxForce, 0, maxForce);

        if (strength < 20f) return;

        Vector3 dir = new Vector3(-stick.x, 0, -stick.y).normalized;

        rb.AddForce(dir * strength * launchMultiplier, ForceMode.Impulse);
    }

    // --------------------
    // MOTORS DEL MANDO
    // --------------------

    void StartVibration(float strength)
    {
        if (pad == null) return;

        pad.SetMotorSpeeds(strength, strength);
        vibrating = true;
    }

    void StopVibration()
    {
        if (pad == null) return;

        pad.SetMotorSpeeds(0, 0);
        vibrating = false;
    }

    public void SetActivePlayer(bool state)
    {
        isActivePlayer = state;

        if (!state)
        {
            lineRenderer.enabled = false;
            StopVibration();
        }
    }
}
