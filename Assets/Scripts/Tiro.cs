using UnityEngine;
using UnityEngine.InputSystem;

public class Tiro : MonoBehaviour
{
    public Rigidbody ballRb;
    public BarraTiro shotBar;

    [Header("Pelota visual")]
    public GameObject ballVisualPrefab;
    private GameObject ballVisualInstance;

    [Header("Fuerza del tiro")]
    public float baseMaxForce = 0f;
    public float baseMinForce = 0f;
    public float arcMultiplier = 0f;

    public Transform hoop;
    public Transform launchPoint;
    public bool HasTheBall = false;

    private Gamepad pad;
    private bool perfectShotPending = false;
    private bool isActivePlayer = false;

    // ======================================================
    // ASIGNACIÓN DEL GAMEPAD DESDE PM
    // ======================================================
    public void SetPad(Gamepad assignedPad)
    {
        pad = assignedPad;
    }

    public void SetActivePlayer(bool active, Gamepad assignedPad)
    {
        isActivePlayer = active;

        if (active)
            pad = assignedPad;
    }

    void Update()
    {
        if (pad == null) return;
        if (!isActivePlayer) return;

        HandleBallVisual();

        if (pad.buttonWest.wasPressedThisFrame && HasTheBall)
        {
            Shoot();
        }
    }

    // ======================================================
    //  CREAR / BORRAR PELOTA VISUAL
    // ======================================================
    void HandleBallVisual()
    {
        if (HasTheBall)
        {
            // Crear si no existe
            if (ballVisualInstance == null && ballVisualPrefab != null)
            {
                ballVisualInstance = Instantiate(
                    ballVisualPrefab,
                    launchPoint.position,
                    launchPoint.rotation,
                    launchPoint
                );
            }
        }
        else
        {
            // Eliminar si existe
            if (ballVisualInstance != null)
            {
                Destroy(ballVisualInstance);
                ballVisualInstance = null;
            }
        }
    }

    // ======================================================
    //                      DISPARO
    // ======================================================
    void Shoot()
    {
        HasTheBall = false;

        // Eliminar la pelota visual
        if (ballVisualInstance != null)
        {
            Destroy(ballVisualInstance);
            ballVisualInstance = null;
        }

        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);
        ballClone.tag = "Bola";

        Vector3 shootDir;

        // --------------------------------------------------
        // 🔥 TIRO PERFECTO
        // --------------------------------------------------
        if (perfectShotPending)
        {
            perfectShotPending = false;

            Vector3 target = hoop.position + Vector3.up * 1.2f;
            shootDir = (target - launchPoint.position).normalized;

            float perfectForce = 12f;
            ballClone.linearVelocity = shootDir * perfectForce;
            return;
        }

        // --------------------------------------------------
        // 🔶 TIRO NORMAL
        // --------------------------------------------------

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(baseMinForce, baseMaxForce, power);

        float finalMultiplier = shotBar != null ? shotBar.ForceMultiplier : 1f;
        force *= finalMultiplier;

        Vector3 targetHoop = hoop.position + Vector3.up * 1.0f;
        shootDir = (targetHoop - launchPoint.position).normalized;

        shootDir = (shootDir + Vector3.up * arcMultiplier).normalized;

        ballClone.linearVelocity = shootDir * force;
    }

    // ======================================================
    //              POTENCIADOR DE TIRO PERFECTO
    // ======================================================
    public void ActivatePerfectShot()
    {
        perfectShotPending = true;
    }
}