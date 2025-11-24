using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Tiro : MonoBehaviour
{
    public Rigidbody ballRb;
    public BarraTiro shotBar;
    public float maxForce = 15f;
    public float minForce = 7.5f;
    public float arcMultiplier = 0.5f;
    public float perfectShotForce = 0f;
    public Transform hoop;
    public Transform launchPoint;

    [Header("Trajectory Settings")]
    public LineRenderer trajectoryLine;
    public int trajectoryResolution = 30;
    public float timeStep = 0.1f;

    [Header("Freeze Settings")]
    public float freezeDuration = 1.5f;
    private bool freezeTrajectory = false;

    void Update()
    {
        transform.LookAt(hoop.position);

        // NO dibujar si está congelada
        if (!freezeTrajectory)
        {
            float power = shotBar.GetPower();
            float force = Mathf.Lerp(minForce, maxForce, power);

            Vector3 shootDirection = (transform.forward * arcMultiplier + Vector3.up).normalized;
            Vector3 startVelocity = shootDirection * force;

            DrawTrajectory(launchPoint.position, startVelocity);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
            StartCoroutine(FreezeTrajectory());
        }
    }

    void Shoot()
    {
        Vector3 direction = hoop.position - transform.position;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = targetRot;
        }

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(minForce, maxForce, power);

        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);
        ballClone.linearVelocity = transform.forward * force;

        Vector3 shootDirection = (transform.forward * arcMultiplier + Vector3.up).normalized;

        ballClone.AddForce(shootDirection * force, ForceMode.Impulse);

        if (shotBar.IsPerfect())
        {
            ShootPerfect(ballClone);
        }
    }

    void ShootPerfect(Rigidbody ball)
    {
        Vector3 target = hoop.position;
        Vector3 direction = (target - ball.transform.position).normalized;

        ball.AddForce(direction * perfectShotForce, ForceMode.Impulse);

        Debug.Log("PERFECT SHOT!");
    }

    // ---- TRAYECTORIA ----
    void DrawTrajectory(Vector3 startPos, Vector3 startVelocity)
    {
        Vector3[] points = new Vector3[trajectoryResolution];

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * timeStep;

            Vector3 point = startPos +
                            startVelocity * t +
                            0.5f * Physics.gravity * t * t;

            points[i] = point;
        }

        trajectoryLine.positionCount = trajectoryResolution;
        trajectoryLine.SetPositions(points);
    }

    void ClearTrajectory()
    {
        trajectoryLine.positionCount = 0;
    }

    // ---- CONGELACIÓN ----
    IEnumerator FreezeTrajectory()
    {
        freezeTrajectory = true;
        // La línea permanece exactamente como estaba 

        yield return new WaitForSeconds(freezeDuration);

        // Se limpia y se reactivará al entrar en Update()
        ClearTrajectory();
        freezeTrajectory = false;
    }
}