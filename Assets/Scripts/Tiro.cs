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

    void Update()
    {
        transform.LookAt(hoop.position);
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
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
            //ShootPerfect(ballClone);
        }
    }

    void ShootPerfect(Rigidbody ball)
    {
        Vector3 target = hoop.position;
        Vector3 direction = (target - ball.transform.position).normalized;

        ball.AddForce(direction * perfectShotForce, ForceMode.Impulse);

        Debug.Log("PERFECT SHOT!");
    }

}