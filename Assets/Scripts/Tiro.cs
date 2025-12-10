using UnityEngine;
using UnityEngine.InputSystem;

public class Tiro : MonoBehaviour
{
    public Rigidbody ballRb;
    public BarraTiro shotBar;

    [Header("Fuerza del tiro")]
    public float baseMaxForce = 10f;
    public float baseMinForce = 4f;

    public float arcMultiplier = 0.15f;

    public Transform hoop;
    public Transform launchPoint;
    public bool HasTheBall = false;

    private Gamepad pad;

    private bool perfectShotPending = false;

    // 🔥 NUEVO: este jugador solo puede tirar si está activo
    private bool isActivePlayer = false;


    // ======================================================
    //             ASIGNACIÓN DEL GAMEPAD DESDE PM
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
        // No hay mando asignado
        if (pad == null) return;

        // ❌ NO puede tirar si no es el jugador activo del equipo
        if (!isActivePlayer) return;

        // Botón de tiro
        if (pad.buttonWest.wasPressedThisFrame)
        {
            Shoot();
        }
    }


    // ======================================================
    //                      DISPARO
    // ======================================================
    void Shoot()
    {
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