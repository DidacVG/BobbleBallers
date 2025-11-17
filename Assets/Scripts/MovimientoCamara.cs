using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoCamara : MonoBehaviour
{
    public Transform ejeHorizontal;
    public Transform ejeVertical;

    public float velocidadRotacion;
    public float limiteRotacion;

    private float rotacionY = 0f;
    private float rotacionX = 0f;

    public GameObject jugador;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        jugador.transform.position = transform.position;
        jugador.transform.rotation = transform.rotation;
        transform.SetParent(jugador.transform, true);

        rotacionY = ejeHorizontal.localEulerAngles.y;
        rotacionX = ejeVertical.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * velocidadRotacion * Time.deltaTime;
        float mouseY = mouseDelta.y * velocidadRotacion * Time.deltaTime;

        rotacionY += mouseX;
        ejeHorizontal.localEulerAngles = new Vector3(0f, rotacionY, 0f);

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -limiteRotacion, limiteRotacion);

        ejeVertical.localEulerAngles = new Vector3(rotacionX, 0f, 0f);
    }
}
