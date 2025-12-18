using UnityEngine;
using System.Collections;

public class ControlesInvertidosPowerUp : MonoBehaviour
{
    public float effectDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NotifyPowerUpConsumed();
        MoverPersonajes player = other.GetComponent<MoverPersonajes>();
        if (player == null) return;

        int targetTeam = (player.team == 0) ? 1 : 0;

        GameManager.Instance.StartInvertControls(targetTeam, effectDuration);

        Destroy(gameObject);
    }
}
