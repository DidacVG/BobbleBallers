using UnityEngine;

public class BallReceiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<MoverPersonajes>();
        if (player != null)
        {
            GameManager.Instance.GiveBallTo(player);
            Destroy(gameObject); // eliminar pelota física
        }
    }
}
