using UnityEngine;
using UnityEngine.InputSystem;

public class Tiro : MonoBehaviour
{
    public Rigidbody ballRb;
    public BarraTiro shotBar;

    public float maxForce = 10f;
    public float minForce = 4f;
    public float arcMultiplier = 0.15f;

    public Transform hoop;
    public Transform launchPoint;

    [Header("Velocidad extra")]
    public float speedBoost = 1.5f;

    [Header("Control de posesión")]
    public bool HasTheBall = true;   // << NUEVO

    void Update()
    {
        transform.LookAt(hoop.position);

        // Si NO tiene la pelota, no puede tirar
        if (!HasTheBall)
            return;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (ballRb == null || launchPoint == null)
        {
            Debug.LogError("BallRb o LaunchPoint no asignados.");
            return;
        }

        // Evita que pueda tirar de nuevo hasta que reciba otra pelota
        HasTheBall = false;

        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(minForce, maxForce, power);

        Vector3 shootDirection = (transform.forward + Vector3.up * arcMultiplier).normalized;

        ballClone.linearVelocity = shootDirection * force * speedBoost;

        Debug.Log("Pelota creada con velocidad: " + ballClone.linearVelocity);
    }
}
