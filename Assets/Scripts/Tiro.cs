using UnityEngine;
using UnityEngine.InputSystem;

public class Tiro : MonoBehaviour
{
    public Rigidbody ballRb;
    public BarraTiro shotBar;
    public float maxForce = 15f;
    public float minForce = 7.5f;
    public float arcMultiplier = 0.5f;
    public float perfectShotForce = 0f;
    public Transform hoop;

    [Header("Trajectory Settings")]
    public LineRenderer trajectoryLine;
    public int trajectoryResolution = 30;
    public float timeStep = 0.1f;

    void Update()
    {
        // Jugador siempre mirando al aro
        transform.LookAt(hoop.position);

        // Cálculo previo a dibujar la trayectoria
        float power = shotBar.GetPower();
        float force = Mathf.Lerp(minForce, maxForce, power);

        Vector3 shootDirection = (transform.forward * arcMultiplier + Vector3.up).normalized;
        Vector3 startVelocity = shootDirection * force;

        // Dibujar curva previa
        DrawTrajectory(ballRb.transform.position, startVelocity);

        // Si dispara
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            ClearTrajectory();
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

        Rigidbody ballClone = Instantiate(ballRb, transform.position, transform.rotation);
        ballClone.linearVelocity = transform.forward * force;

        Vector3 shootDirection = (transform.forward * arcMultiplier + Vector3.up).normalized;

        ballClone.AddForce(shootDirection * force, ForceMode.Impulse);

        if (shotBar.IsPerfect())
        {
            ShootPerfect(ballClone);
            return;
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
}