using UnityEngine;

public class DoublePointsPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NotifyPowerUpConsumed();
        MoverPersonajes player = other.GetComponent<MoverPersonajes>();
        if (player == null) return;

        GameManager.Instance.ActivateDoublePoints(player.team);

        // Feedback visual / sonoro opcional
        Debug.Log($"🟣 Power-Up x2 recogido por equipo {player.team}");

        Destroy(gameObject);
    }
}