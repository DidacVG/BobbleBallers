using UnityEngine;

public class SlowShotPowerUp : MonoBehaviour
{
    [Header("Efecto")]
    public float slowMultiplier = 0.5f;   // 40% velocidad
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.NotifyPowerUpConsumed();
        // Buscar BarraTiro en el jugador
        BarraTiro barra = other.GetComponentInChildren<BarraTiro>();

        if (barra == null)
            return;

        // Aplicar efecto
        barra.ApplySlow(slowMultiplier, duration);

        // Destruir el objeto
        Destroy(gameObject);
    }
}
