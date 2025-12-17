using UnityEngine;

public class DetectarSalidaCampo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        Destroy(other.gameObject);

        GameManager.Instance.OnBallOut();

        Debug.Log("🚨 BALÓN FUERA");
    }
}
