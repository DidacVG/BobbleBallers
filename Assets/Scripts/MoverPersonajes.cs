using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    private Tiro tiro;

    private bool isActivePlayer = false;
    private bool dragging = false;

    private Vector2 storedDirection;
    private float storedMagnitude = 0;

    private Gamepad pad;

    public GameObject selectionCirclePrefab;
    private GameObject selectionCircleInstance;

    public int team;   // 0 = Equipo A, 1 = Equipo B

    public bool HasTheBall = false;

    [Header("Robo de balón")]
    public bool canSteal = true;
    public float stealCooldown = 0.5f;

    private bool invertControls = false;

    [Header("Freeze")]
    public bool isFrozen = false;
    private Coroutine freezeRoutine;
    public Color freezeColor = Color.cyan;

    private Renderer[] renderers;
    private MaterialPropertyBlock mpb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        renderers = GetComponentsInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        // Círculo de selección
        if (selectionCirclePrefab != null)
        {
            selectionCircleInstance = Instantiate(selectionCirclePrefab, transform);
            selectionCircleInstance.transform.localPosition = Vector3.zero + Vector3.up * 3f;
            selectionCircleInstance.SetActive(false);
        }
    }

    public void SetInvertControls(bool value)
    {
        invertControls = value;

        // 🔥 RESET TOTAL DEL INPUT
        dragging = false;
        storedMagnitude = 0f;
        storedDirection = Vector2.zero;

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        StopVibration();
    }

    void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused())
            return;
        if (!isActivePlayer || pad == null || isFrozen) return;

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

        if (invertControls)
        {
            stick = -stick;
            StartVibration(0.8f);
        }

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

            // Registrar dirección y magnitud máximas
            if (stick.magnitude > 0.9f)
            {
                storedMagnitude = stick.magnitude;
                storedDirection = stick.normalized;  // ← Ya viene invertido si toca
            }
        }

        if (HasTheBall)
            Debug.Log($"{name} TIENE EL BALÓN");
    }

    // --------------------------
    // DIBUJAR LÍNEA
    // --------------------------
    void DrawLine(Vector2 dir, float mag)
    {
        float strength = Mathf.Clamp(mag * maxForce, 0, maxForce);
        Vector3 start = rb.transform.position + Vector3.up * 0.1f;
        Vector3 end = start + new Vector3(dir.x, 0, dir.y) * (strength / 40f);

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

        if (rb.linearVelocity.magnitude > maxVelocity)
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Solo funciona si choca con un muro
        if (collision.collider.CompareTag("Muro"))
        {
            Vector3 vel = rb.linearVelocity;

            // Invertimos movimiento horizontal (pero NO vertical)
            vel.x = -vel.x;
            vel.z = -vel.z;

            rb.linearVelocity = vel;

            // Opcional: pequeño impulso extra para evitar quedarse pegado al muro
            rb.AddForce(-vel.normalized * 2f, ForceMode.Impulse);
        }

        // --- ROBO DE BALÓN ---
        MoverPersonajes other = collision.collider.GetComponent<MoverPersonajes>();
        if (other == null) return;

        // No robar a compañeros
        if (other.team == team) return;

        Tiro otherTiro = other.GetComponent<Tiro>();
        if (otherTiro == null || tiro == null) return;

        // Robo válido
        if (canSteal && otherTiro.HasTheBall && !tiro.HasTheBall)
        {
            RobarBalon(otherTiro);
        }
    }

    void RobarBalon(Tiro victimTiro)
    {
        Debug.Log($"{name} roba el balón");

        victimTiro.HasTheBall = false;
        tiro.HasTheBall = true;

        StartCoroutine(StealCooldown());
    }

    IEnumerator StealCooldown()
    {
        canSteal = false;
        yield return new WaitForSeconds(stealCooldown);
        canSteal = true;
    }

    public void CreateBallVisual()
    {
        HasTheBall = true;
    }

    public void RemoveBallVisual()
    {
        HasTheBall = false;
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

    public void TakeBallFrom(MoverPersonajes other)
    {
        if (other == null) return;

        other.HasTheBall = false;
        HasTheBall = true;

        Debug.Log($"{name} ROBA el balón a {other.name}");
    }

    public void Freeze(float duration)
    {
        if (freezeRoutine != null)
            StopCoroutine(freezeRoutine);

        freezeRoutine = StartCoroutine(FreezeRoutine(duration));
    }

    IEnumerator FreezeRoutine(float duration)
    {
        isFrozen = true;

        rb.linearVelocity = Vector3.zero;
        ApplyFreezeColor();

        dragging = false;
        storedMagnitude = 0f;
        storedDirection = Vector2.zero;

        if (lineRenderer != null)
            lineRenderer.enabled = false;

        StopVibration();

        yield return new WaitForSeconds(duration);

        isFrozen = false;
        ClearFreezeColor();
        freezeRoutine = null;
    }
    void ApplyFreezeColor()
    {
        foreach (Renderer r in renderers)
        {
            r.GetPropertyBlock(mpb);
            mpb.SetColor("_Color", freezeColor);
            r.SetPropertyBlock(mpb);
        }
    }
    void ClearFreezeColor()
    {
        foreach (Renderer r in renderers)
        {
            r.SetPropertyBlock(null);
        }
    }
}