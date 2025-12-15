using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public bool isThreePointZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        BallData data = other.GetComponent<BallData>();
        if (data == null) return;

        // 🔒 EVITAR DOBLES ANOTACIONES
        if (data.hasScored) return;

        data.hasScored = true;

        int points = isThreePointZone ? 3 : 2;

        Debug.Log("SUMANDO PUNTOS: " + points);

        GameManager.Instance.OnScore(
            data.lastShooter,
            points,
            true
        );
    }
}
