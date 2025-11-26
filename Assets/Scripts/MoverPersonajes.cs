using UnityEngine;

public class MoverPersonajes : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public float launchMultiplier = 5f;
    public float maxForce = 300f;
    public float minVelocityToMove = 0.1f;
    public float rotationSpeed = 10f;  // Velocidad de rotación

    public Rigidbody rb;
    public LineRenderer lineRenderer;

    private Vector2 startPos;
    private Vector2 currentPos;
    private bool dragging = false;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Obtener velocidad real del rigidbody
        Vector3 currentVel = rb.IsSleeping() ? Vector3.zero : rb.GetPointVelocity(rb.worldCenterOfMass);

        // ---- ROTACIÓN HACIA DIRECCIÓN DE MOVIMIENTO ----
        if (currentVel.sqrMagnitude > minVelocityToMove * minVelocityToMove)
        {
            Vector3 horizontalVel = new Vector3(currentVel.x, 0, currentVel.z);

            // Girar hacia la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVel, Vector3.up);

            // ---- INCLINACIÓN HACIA ATRÁS SEGÚN VELOCIDAD ----
            float speed = horizontalVel.magnitude;
            float maxTilt = 30f; // grados máximos hacia atrás
            float tiltAmount = Mathf.Lerp(0, maxTilt, speed / 10f); // 10 = velocidad a la que alcanza el máximo

            Quaternion tiltRotation = Quaternion.Euler(tiltAmount, 0, 0);

            // Combinar giro + inclinación
            Quaternion finalRotation = targetRotation * tiltRotation;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                finalRotation,
                Time.deltaTime * rotationSpeed
            );

            // No permitir arrastrar mientras se mueve
            dragging = false;
            lineRenderer.enabled = false;
            return;
        }

        // --- SI ESTÁ QUIETO → PERMITE ARRASTRAR ---

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            startPos = Input.mousePosition;
            lineRenderer.enabled = true;
        }

        if (dragging)
        {
            currentPos = Input.mousePosition;
            Vector2 drag = currentPos - startPos;
            DrawLine(drag);
        }

        if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            lineRenderer.enabled = false;

            Vector2 drag = currentPos - startPos;
            LaunchOpposite(drag);
        }
    }

    void DrawLine(Vector2 drag)
    {
        float strength = Mathf.Clamp(drag.magnitude, 0, maxForce);
        Vector2 opposite = -drag.normalized;

        Vector3 start = rb.transform.position;
        Vector3 end = start + new Vector3(opposite.x, 0f, opposite.y) * (strength / 40f);

        lineRenderer.SetPosition(0, start + Vector3.up * 0.1f);
        lineRenderer.SetPosition(1, end + Vector3.up * 0.1f);
    }

    void LaunchOpposite(Vector2 drag)
    {
        if (drag.magnitude < 20f)
            return;

        float strength = Mathf.Clamp(drag.magnitude, 0, maxForce);

        Vector2 opposite = -drag.normalized;
        Vector3 dir = new Vector3(opposite.x, 0f, opposite.y);

        rb.AddForce(dir * strength * launchMultiplier);

        Debug.Log("Launch direction: " + dir + " / strength = " + strength);
    }
}
