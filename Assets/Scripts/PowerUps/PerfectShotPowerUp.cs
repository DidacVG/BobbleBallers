using UnityEngine;

public class PerfectShotPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Tiro tiro = other.GetComponent<Tiro>();

        if (tiro != null)
        {
            tiro.ActivatePerfectShot();
            Destroy(gameObject); // La caja desaparece
        }
    }
}
