using UnityEngine;
using UnityEngine.InputSystem;

public class PaseBalon : MonoBehaviour
{
    public Tiro tiro;                     // Script de tiro
    public Rigidbody ballPrefab;          // Prefab de la pelota
    public Transform[] teammates;         // Compañeros de equipo

    public float passForce = 8f;
    public float passArc = 0.1f;
    public float maxAngle = 45f;          // Ángulo máximo para considerar el pase

    private Gamepad pad;

    void Start()
    {
        tiro = GetComponent<Tiro>();
    }

    public void SetGamepad(Gamepad assignedPad)
    {
        pad = assignedPad;
    }

    void Update()
    {
        if (pad == null) return;

        // ❌ NO puedes pasar sin la pelota
        if (!tiro.HasTheBall) return;

        // Botón R1 del mando PS5 / Xbox
        if (pad.buttonSouth.wasPressedThisFrame)
        {
            PasarConDireccion();
        }
    }

    void PasarConDireccion()
    {
        Vector2 stick = pad.rightStick.ReadValue();

        // ❌ Si el stick no se está moviendo, no hay dirección
        if (stick.magnitude < 0.3f)
            return;

        // Dirección del stick en 3D
        Vector3 dir = new Vector3(stick.x, 0f, stick.y).normalized;

        Transform mejorObjetivo = SeleccionarJugadorDirigido(dir);

        if (mejorObjetivo == null)
            return;

        tiro.HasTheBall = false;

        // Crear la pelota
        Rigidbody ballClone = Instantiate(ballPrefab, tiro.launchPoint.position, Quaternion.identity);
        ballClone.tag = "Bola";

        // Direccion de pase
        Vector3 passDir = ((mejorObjetivo.position - transform.position).normalized + Vector3.up * passArc).normalized;

        ballClone.linearVelocity = passDir * passForce;

        // Asignar posesión al receptor
        Tiro tiroReceptor = mejorObjetivo.GetComponent<Tiro>();
        if (tiroReceptor != null)
            tiroReceptor.HasTheBall = true;
    }

    Transform SeleccionarJugadorDirigido(Vector3 inputDir)
    {
        Transform mejor = null;
        float mejorAngulo = maxAngle;

        foreach (Transform t in teammates)
        {
            if (t == transform) continue;

            Vector3 toMate = (t.position - transform.position).normalized;

            float ang = Vector3.Angle(inputDir, toMate);

            // Solo selecciona si está dentro del ángulo del pase
            if (ang < mejorAngulo)
            {
                mejorAngulo = ang;
                mejor = t;
            }
        }

        return mejor;
    }
}
