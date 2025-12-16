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

    [Header("Posesión de pelota")]
    public bool HasTheBall = false;

    private Gamepad pad;
    private bool perfectShotPending = false;
    private bool isActivePlayer = false;

    public MoverPersonajes owner;   // ← ASIGNAR EN INSPECTOR


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

        // Disparo solo si tengo la pelota
        if (pad.buttonWest.wasPressedThisFrame && HasTheBall)
        {
            transform.LookAt(hoop);
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
            if (ballVisualInstance != null)
            {
                Destroy(ballVisualInstance);
                ballVisualInstance = null;
            }
        }
    }


    // ======================================================
    // ✋ DETECCIÓN DE COLISIÓN CON LA PELOTA REAL
    // ======================================================
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bola"))
        {
            CatchBall(collision.collider.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bola"))
        {
            CatchBall(other.gameObject);
        }
    }


    // ======================================================
    // ✔ RECOGER PELOTA
    // ======================================================
    void CatchBall(GameObject ballObj)
    {
        // Obtener la pelota
        Rigidbody ball = ballObj.GetComponent<Rigidbody>();
        if (ball == null) return;

        // Marcar posesión
        HasTheBall = true;

        // Eliminar la pelota del suelo
        Destroy(ballObj);

        // Crear pelota visual inmediatamente
        HandleBallVisual();
    }


    // ======================================================
    //                     DISPARO
    // ======================================================
    void Shoot()
    {
        HasTheBall = false;

        if (ballVisualInstance != null)
        {
            Destroy(ballVisualInstance);
            ballVisualInstance = null;
        }

        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);
        ballClone.tag = "Bola";

        BallData data = ballClone.GetComponent<BallData>();
        if (data == null)
            data = ballClone.gameObject.AddComponent<BallData>();

        MoverPersonajes mover = GetComponent<MoverPersonajes>();

        if (mover == null)
        {
            Debug.LogError("❌ Tiro NO está en un objeto con MoverPersonajes");
            return;
        }

        data.lastShooter = mover;
        data.hasScored = false;
        data.wasPerfectShot = perfectShotPending;

        Debug.Log("Shooter asignado correctamente: " + mover.name);

        Vector3 targetHoop = hoop.position + Vector3.up * 1.0f;
        Vector3 shootDir = (targetHoop - launchPoint.position).normalized;
        shootDir = (shootDir + Vector3.up * arcMultiplier).normalized;

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(baseMinForce, baseMaxForce, power);

        ballClone.linearVelocity = shootDir * force;
    }


    // ======================================================
    // POTENCIADOR DE TIRO PERFECTO
    // ======================================================
    public void ActivatePerfectShot()
    {
        perfectShotPending = true;
    }
}