using UnityEngine;

public class DetectarSalidaCampo : MonoBehaviour
{
    public Tiro shooter;                      // Script del jugador que tira
    public Transform ballRespawnPosition;     // Posición donde reaparece la pelota
    public MoverPersonajes[] players;         // Para resetear sus posiciones
    private Vector3[] initialPositions;

    void Start()
    {
        // Guardamos las posiciones iniciales de los jugadores
        initialPositions = new Vector3[players.Length];
        for (int i = 0; i < players.Length; i++)
            initialPositions[i] = players[i].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bola"))
        {
            Destroy(other.gameObject);   // ❌ Borrar pelota

            // 👉 Devolvemos la posesión al jugador
            shooter.HasTheBall = true;

            // 👉 Reset posiciones jugadores
            ResetPlayerPositions();

            // 👉 (Opcional) Debug
            Debug.Log("Pelota fuera. Jugadores reseteados y posesión devuelta.");
        }
    }

    void ResetPlayerPositions()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].rb.linearVelocity = Vector3.zero;
            players[i].rb.angularVelocity = Vector3.zero;

            players[i].transform.position = initialPositions[i];
        }
    }
}
