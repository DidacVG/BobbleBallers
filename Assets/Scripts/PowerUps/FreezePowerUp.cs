using UnityEngine;

public class FreezePowerUp : MonoBehaviour
{
    public float freezeDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NotifyPowerUpConsumed();
        MoverPersonajes picker = other.GetComponent<MoverPersonajes>();
        if (picker == null) return;

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("❌ PlayerManager.Instance no existe");
            return;
        }

        // Seleccionar equipo rival
        MoverPersonajes[] enemies =
            (picker.team == 0)
            ? PlayerManager.Instance.teamB
            : PlayerManager.Instance.teamA;

        // Congelar a todo el equipo rival
        foreach (MoverPersonajes enemy in enemies)
        {
            if (enemy != null)
                enemy.Freeze(freezeDuration);
        }

        // Feedback opcional (FX / sonido)
        Destroy(gameObject);
    }
}
