using UnityEngine;

public class SlowShotPowerUp : MonoBehaviour
{
    public float multiplier = 0.4f;
    public float duration = 3f;

    void OnTriggerEnter(Collider other)
    {
        BarraTiro bar = other.GetComponentInChildren<BarraTiro>();
        if (bar != null)
        {
            bar.ApplyTemporarySlow(multiplier, duration);
            Destroy(gameObject);
        }
    }
}
