using UnityEngine;

public class BallReceiver : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        PaseBalon pass = col.collider.GetComponent<PaseBalon>();

        if (pass != null)
        {
            pass.HasTheBall = true;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;

            transform.SetParent(pass.transform);
            transform.localPosition = new Vector3(0, 1, 0); // posición en mano

            Debug.Log("El jugador " + pass.name + " recibe la pelota");
        }
    }
}
