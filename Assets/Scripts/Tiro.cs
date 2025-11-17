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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        transform.LookAt(hoop.position);
    }

    void Shoot()
    {
        Vector3 direction = hoop.position - transform.position;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = targetRot;
        }

        float power = shotBar.GetPower(); // 0 a 1
        float force = power * maxForce;

        Rigidbody ballClone = (Rigidbody)Instantiate(ballRb, transform.position, transform.rotation);
        ballClone.linearVelocity = transform.forward * force;

        Vector3 shootDirection = (transform.forward + Vector3.up * arcMultiplier).normalized;

        ballRb.AddForce(shootDirection * force, ForceMode.Impulse);

        if (shotBar.IsPerfect())
        {
            ShootPerfect();
            return;
        }
    }

        // Si no es perfecto, tiro normal

    void ShootPerfect()
    {
        Vector3 target = hoop.position;

        // Dirección hacia la canasta
        Vector3 direction = (target - ballRb.transform.position).normalized;

        float perfectForce = perfectShotForce; // define una fuerza especial
        ballRb.AddForce(direction * perfectForce, ForceMode.Impulse);

        Debug.Log("PERFECT SHOT!");
    }
}
