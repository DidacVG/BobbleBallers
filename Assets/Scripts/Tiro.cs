using UnityEngine;
using UnityEngine.InputSystem;

public class Tiro : MonoBehaviour
{
    [Header("Lanzamiento")]
    public float maxForce = 0f;
    public float minForce = 0f;
    public float arcMultiplier = 0f;

    [Header("Referencias")]
    public Rigidbody ballRb;
    public Transform hoop;
    public Transform launchPoint;
    public BarraTiro shotBar;

    [Header("Estado")]
    public bool HasTheBall = true;

    // internos
    private bool isActivePlayer = false;
    private Gamepad pad;  // ← mando asignado por PlayerManager

    private void Update()
    {
        if (!isActivePlayer) return;
        if (!HasTheBall) return;

        // Mirar hacia la canasta siempre
        transform.LookAt(hoop.position);
    }

    // ----------------------
    // INPUT ACTION: Shoot
    // ----------------------
    public void OnShoot(InputAction.CallbackContext ctx)
    {
        if (!isActivePlayer) return;
        if (!HasTheBall) return;
        if (!ctx.performed) return;

        Shoot();
    }

    // ----------------------
    // Recibe mando + activación desde PlayerManager
    // ----------------------
    public void SetActivePlayer(bool state, Gamepad assignedPad = null)
    {
        isActivePlayer = state;

        if (assignedPad != null)
            pad = assignedPad;
    }

    // ----------------------
    // LÓGICA DEL TIRO
    // ----------------------
    private void Shoot()
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
