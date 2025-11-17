using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public DetectarSuelo detectorSuelo;
    public float velocidadMovimiento = 0f;
    public float fuerzaSalto = 0f;

    private Rigidbody rb;
    private Vector2 moveInput = Vector2.zero;

    public float MultiplicadorSprint = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 0.5f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 0.75f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 0.75f;

        if (detectorSuelo.Grounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            Debug.Log("brinca");
        }
    }

    void FixedUpdate()
    {
        if (detectorSuelo.Grounded) 
        {
            if (Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                Vector3 direccion = transform.forward * moveInput.y + transform.right * moveInput.x;

                if (direccion.sqrMagnitude > 1f)
                {
                    direccion = direccion.normalized;
                }

                Vector3 movimiento = direccion * velocidadMovimiento * MultiplicadorSprint * Time.fixedDeltaTime;
                Vector3 nuevaPos = rb.position + movimiento;
                nuevaPos.y = rb.position.y;
                Debug.Log("sprint");
                rb.MovePosition(nuevaPos);
            }
            else
            {
                Vector3 direccion = transform.forward * moveInput.y + transform.right * moveInput.x;

                if (direccion.sqrMagnitude > 1f)
                {
                    direccion = direccion.normalized;
                }

                Vector3 movimiento = direccion * velocidadMovimiento * MultiplicadorSprint * Time.fixedDeltaTime;
                Vector3 nuevaPos = rb.position + movimiento;
                nuevaPos.y = rb.position.y;
                Debug.Log(movimiento);
                rb.MovePosition(nuevaPos);
            }
        }
        
    }
    
}

