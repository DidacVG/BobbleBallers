using UnityEngine;
using UnityEngine.InputSystem;

public class Jetpack : MonoBehaviour
{
    private Rigidbody rb;
    public DetectarSuelo detectorSuelo;
    public float fuerzaJetpack = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectorSuelo.Grounded == false && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * fuerzaJetpack, ForceMode.Impulse);
            Debug.Log("jetpack");
        }
    }
}
