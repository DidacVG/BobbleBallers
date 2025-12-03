using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    public bool isThreePointZone = false;   // Si este collider es la zona de 3 puntos

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        // Saber quién lanzó el balón
        Tiro tiro = other.GetComponent<BallData>()?.shooter;

        if (tiro == null) return;

        bool isTeamA = tiro.GetComponent<MoverPersonajes>().team == 0;

        // Puntos según zona
        int points = isThreePointZone ? 3 : 2;

        ScoreManager.Instance.AddPoints(isTeamA, points);

        Debug.Log("Anotado: " + points + " puntos");
    }
}
