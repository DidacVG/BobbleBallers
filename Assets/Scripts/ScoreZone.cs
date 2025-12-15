using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        BallData data = other.GetComponent<BallData>();
        if (data == null) return;

        // 🔒 Bloqueo total: ya anotó
        if (data.hasScored) return;

        data.hasScored = true;

        // ✅ SUMA 1 PUNTO SIEMPRE
        GameManager.Instance.OnScore(
            data.lastShooter,
            0,
            true
        );

        Debug.Log("🏀 CANASTA: +1 punto");
    }
}
