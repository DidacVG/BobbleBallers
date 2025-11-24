using UnityEngine;

public class MoverPersonajes : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public float launchMultiplier = 5f;
    public float maxForce = 300f;
    public float minVelocityToMove = 0.1f;

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
        // Comprobación segura de velocidad: usamos GetPointVelocity y comprobamos sqrMagnitude
        Vector3 currentVel = rb.IsSleeping() ? Vector3.zero : rb.GetPointVelocity(rb.worldCenterOfMass);
        if (currentVel.sqrMagnitude > minVelocityToMove * minVelocityToMove)
        {
            dragging = false;
            lineRenderer.enabled = false;
            return;
        }

        // Iniciar arrastre
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            startPos = Input.mousePosition;
            lineRenderer.enabled = true;
        }

        // Mientras arrastras → dibujar línea
        if (dragging)
        {
            currentPos = Input.mousePosition;
            Vector2 drag = currentPos - startPos;
            DrawLine(drag);
        }

        // Soltar → lanzar y ocultar línea
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

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
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
