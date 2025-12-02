using UnityEngine;

public class BallReceiver : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PaseBalon player = other.GetComponent<PaseBalon>();

        if (player != null)
        {
            // Darle el balón al jugador que lo tocó
            player.HasTheBall = true;

            // Quitar el balón del resto
            DesactivarBalonOtros(player);

            Destroy(gameObject); // eliminar la pelota física
        }
    }

    void DesactivarBalonOtros(PaseBalon nuevoDueño)
    {
        PaseBalon[] all = FindObjectsOfType<PaseBalon>();

        foreach (var p in all)
        {
            if (p != nuevoDueño)
                p.HasTheBall = false;
        }
    }
}
