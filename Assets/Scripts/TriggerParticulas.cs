using UnityEngine;

public class TriggerParticulas : MonoBehaviour
{
    public ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bola")) return;

        particles.transform.position = other.transform.position;
        particles.Stop();
        particles.Play();
    }
}
