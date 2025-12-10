using UnityEngine;
using System.Collections;

public class ControlesInvertidosPowerUp : MonoBehaviour
{
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        MoverPersonajes player = other.GetComponent<MoverPersonajes>();
        if (player == null) return;

        int enemyTeam = (player.team == 0 ? 1 : 0);

        StartCoroutine(InvertTeam(enemyTeam));
        Destroy(gameObject);
    }

    IEnumerator InvertTeam(int teamToInvert)
    {
        MoverPersonajes[] all = FindObjectsOfType<MoverPersonajes>();

        foreach (var p in all)
            if (p.team == teamToInvert)
                p.invertedControls = true;

        yield return new WaitForSeconds(duration);

        foreach (var p in all)
            if (p.team == teamToInvert)
                p.invertedControls = false;
    }
}
