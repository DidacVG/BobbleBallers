using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    public float distanciaMaxima = 0f;
    public LayerMask pickupLayer;
    public Transform puntoAgarre;

    private Rigidbody objetoActual;
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (objetoActual == null)
            {
                IntentarRecogerObjeto();
            }
        }
        else
        {
            SoltarObjeto();
        }
    }

    void IntentarRecogerObjeto()
    {
        Ray rayo = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(rayo.origin, rayo.direction * distanciaMaxima, Color.blue, 1f);
        if (Physics.Raycast(rayo, out RaycastHit hit, distanciaMaxima, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null && objetoActual == null)
            {
                objetoActual = rb;
                objetoActual.useGravity = false;
                objetoActual.linearVelocity = Vector3.zero;
                objetoActual.angularVelocity = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if (objetoActual != null)
        {
            Vector3 newPos = Vector3.Lerp(objetoActual.position, puntoAgarre.position, 20f * Time.fixedDeltaTime);
            objetoActual.MovePosition(newPos);
        }
    }

    void SoltarObjeto()
    {
        if (objetoActual != null)
        {
            objetoActual.useGravity = true;
            objetoActual = null;
        }
    }
}
