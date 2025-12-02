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

    public bool HasTheBall = true;   // << POSESIÓN

    private Gamepad pad;

    void Start()
    {
        pad = Gamepad.current;
    }

    void Update()
    {
        if (pad == null) return;

        transform.LookAt(hoop.position);

        // ❌ NO puedes tirar si no tienes la pelota
        if (!HasTheBall) return;

        // Botón CUADRADO del mando PS5 = buttonWest
        if (pad.buttonWest.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        HasTheBall = false;

        Rigidbody ballClone = Instantiate(ballRb, launchPoint.position, launchPoint.rotation);
        ballClone.tag = "Bola";   // Asegurarse tag correcto

        float power = shotBar.GetPower();
        float force = Mathf.Lerp(minForce, maxForce, power);

        Vector3 shootDirection = (transform.forward + Vector3.up * arcMultiplier).normalized;

        ballClone.linearVelocity = shootDirection * force;
    }
}
