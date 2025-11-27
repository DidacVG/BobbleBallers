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

    void Update()
    {
        // Mirar siempre al aro
        transform.LookAt(hoop.position);

        // Detectar espacio con Input System nuevo
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

        // Crear pelota
        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(minForce, maxForce, power);

        // Dirección con un poco de arco
        Vector3 shootDirection = (transform.forward + Vector3.up * arcMultiplier).normalized;

        // NUEVO SISTEMA → usar linearVelocity
        ballClone.linearVelocity = shootDirection * force;

        Debug.Log("Pelota creada y lanzada con linearVelocity: " + ballClone.linearVelocity);
    }
}
