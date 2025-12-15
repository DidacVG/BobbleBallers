using UnityEngine;

public class TriggerParticulas : MonoBehaviour
{
    public ParticleSystem particles;
    public int points = 2; // este trigger vale 2 o 3

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        particles.transform.position = other.transform.position;
        particles.Stop();
        particles.Play();

        BallData data = other.GetComponent<BallData>();
        if (data != null && data.lastShooter != null)
        {
            GameManager.Instance.OnScore(
                data.lastShooter,
                points,
                data.wasPerfectShot
            );
        }

        Destroy(other.gameObject);
    }
}
