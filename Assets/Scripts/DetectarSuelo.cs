using UnityEngine;

public class DetectarSuelo : MonoBehaviour
{
    public float distanciaRaycast = 0.3f;
    public LayerMask capaSuelo;

    public bool mostrarRaycast = true;

    public bool Grounded { get; private set; }

    void Update()
    {
        // Lanza un rayo hacia abajo desde la posición actual
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distanciaRaycast, capaSuelo))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        // Dibuja el raycast en la escena (solo visible en modo editor)
        if (mostrarRaycast)
        {
            Color color = Grounded ? Color.green : Color.red;
            Debug.DrawRay(transform.position, Vector3.down * distanciaRaycast, color);
        }
    }
}
