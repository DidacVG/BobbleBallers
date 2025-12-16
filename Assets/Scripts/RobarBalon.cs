using UnityEngine;
using System.Collections;

public class RobarBalon : MonoBehaviour
{
    private MoverPersonajes self;
    private bool canSteal = true;

    void Awake()
    {
        self = GetComponent<MoverPersonajes>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canSteal) return;

        MoverPersonajes other = collision.collider.GetComponent<MoverPersonajes>();
        if (other == null) return;

        if (other.HasTheBall && !self.HasTheBall)
        {
            Debug.Log($"{self.name} roba el balón a {other.name}");

            GameManager.Instance.GiveBallTo(self); // ✅ CLAVE
            StartCoroutine(StealCooldown());
        }
    }

    IEnumerator StealCooldown()
    {
        canSteal = false;
        yield return new WaitForSeconds(0.5f);
        canSteal = true;
    }
}


