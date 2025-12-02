using UnityEngine;
using UnityEngine.InputSystem;

public class PaseBalon : MonoBehaviour
{
    public bool HasTheBall = false;            // Si este jugador tiene el balón
    public Transform passPoint;                // Punto desde donde se lanza el pase
    public Rigidbody ballPrefab;               // Prefab de la pelota
    public float passForce = 12f;              // Fuerza del pase

    private Gamepad pad;

    void Start()
    {
        pad = Gamepad.current;
    }

    void Update()
    {
        if (!HasTheBall) return;
        if (pad == null) return;

        Vector2 passInput = pad.rightStick.ReadValue();

        // solo si el stick derecho se mueve lo suficiente
        if (passInput.magnitude > 0.35f)
        {
            PassBall(passInput);
        }
    }

    void PassBall(Vector2 dir)
    {
        HasTheBall = false;

        // crear balón
        Rigidbody newBall = Instantiate(ballPrefab, passPoint.position, Quaternion.identity);

        // convertir stick en dirección del mundo
        Vector3 worldDir = new Vector3(dir.x, 0, dir.y).normalized;

        // lanzar balón
        newBall.linearVelocity = worldDir * passForce;

        Debug.Log("Pase realizado hacia: " + worldDir);
    }
}
