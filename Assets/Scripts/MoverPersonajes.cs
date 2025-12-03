using UnityEngine;
using UnityEngine.InputSystem;

public class MoverPersonajes : MonoBehaviour
{
    public float launchMultiplier = 3f;
    public float maxForce = 80f;
    public float maxVelocity = 10f;
    public float minDragThreshold = 0.25f;
    public float releaseThreshold = 0.2f;
    public float rotationSpeed = 10f;
    public float maxTilt = 30f;

    public Rigidbody rb;
    public LineRenderer lineRenderer;

    private bool isActivePlayer = false;
    private bool dragging = false;

    private Vector2 storedDirection;       // ← dirección máxima memorizada
    private float storedMagnitude = 0;     // ← magnitud máxima memorizada

    private Gamepad pad;

    public GameObject selectionCirclePrefab;
    private GameObject selectionCircleInstance;

    public int team;   // 0 = Equipo A, 1 = Equipo B


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        // Crear círculo de selección
        if (selectionCirclePrefab != null)
        {
            selectionCircleInstance = Instantiate(selectionCirclePrefab, transform);
            selectionCircleInstance.transform.localPosition = Vector3.zero + Vector3.up * 3f;
            selectionCircleInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (!isActivePlayer || pad == null) return;

        Vector3 vel = rb.linearVelocity;

        // ---------------- ROTACIÓN MIENTRAS SE MUEVE ----------------
        if (vel.sqrMagnitude > 0.05f)
        {
            Vector3 hVel = new Vector3(vel.x, 0, vel.z);
            Quaternion lookRot = Quaternion.LookRotation(hVel);
            float tilt = Mathf.Lerp(0, maxTilt, hVel.magnitude / maxVelocity);
            Quaternion tiltRot = Quaternion.Euler(tilt, 0, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot * tiltRot, Time.deltaTime * rotationSpeed);

            dragging = false;
            lineRenderer.enabled = false;
            StopVibration();
            return;
        }

        // ---------------- INPUT DEL JOYSTICK ----------------
        Vector2 stick = pad.leftStick.ReadValue();

        // Inicio de arrastre
        if (!dragging && stick.magnitude > minDragThreshold)
        {
            dragging = true;
            storedMagnitude = 0;
            storedDirection = Vector2.zero;

            lineRenderer.enabled = true;
        }

        // Arrastre
        if (dragging)
        {

            DrawLine(storedDirection, storedMagnitude);

            // Lanzar cuando vuelve a neutro
            if (stick.magnitude < releaseThreshold)
            {
                Launch();
                dragging = false;
                lineRenderer.enabled = false;
                StopVibration();
            }
            if (stick.magnitude > 0.9f)
            {
                storedMagnitude = stick.magnitude;
                storedDirection = stick.normalized;
            }
        }
    }

    // --------------------------
    // DIBUJAR LÍNEA
    // --------------------------
    void DrawLine(Vector2 dir, float mag)
    {
        float strength = Mathf.Clamp(mag * maxForce, 0, maxForce);
        Vector2 opposite = dir;

        Vector3 start = rb.transform.position + Vector3.up * 0.1f;
        Vector3 end = start + new Vector3(opposite.x, 0, opposite.y) * (strength / 40f);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        float vib = Mathf.Clamp(mag, 0, 1);
        StartVibration(vib);
    }

    // --------------------------
    // LANZAMIENTO CONSISTENTE
    // --------------------------
    void Launch()
    {
        if (storedMagnitude < minDragThreshold) return;

        float strength = Mathf.Clamp(storedMagnitude * maxForce, 0, maxForce);
        Vector3 forceDir = new Vector3(storedDirection.x, 0, storedDirection.y);

        rb.AddForce(forceDir * strength * launchMultiplier, ForceMode.Impulse);

        // Limitar velocidad máxima
        if (rb.linearVelocity.magnitude > maxVelocity)
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
    }

    // -------------------- VIBRACIÓN --------------------
    void StartVibration(float strength)
    {
        if (pad == null) return;
        pad.SetMotorSpeeds(strength, strength);
    }

    void StopVibration()
    {
        if (pad == null) return;
        pad.SetMotorSpeeds(0, 0);
    }

    public void SetActivePlayer(bool state, Gamepad assignedPad = null)
    {
        isActivePlayer = state;

        // Asignar mando
        if (assignedPad != null)
            pad = assignedPad;

        if (selectionCircleInstance != null)
            selectionCircleInstance.SetActive(state);

        if (!state)
        {
            lineRenderer.enabled = false;
            StopVibration();
        }
    }
}
