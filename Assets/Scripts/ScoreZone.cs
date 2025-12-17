using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        BallData data = other.GetComponent<BallData>();
        if (data == null || data.hasScored) return;

        data.hasScored = true;

        GameManager.Instance.OnScore(data.lastShooter, 2);

        GameManager.Instance.ClearBall();

        Destroy(other.gameObject);
    }
}
