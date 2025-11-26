using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    private bool IsBallFlying;
    public PlayerController player;
    public Vector3 BallPosition;
    public Vector3 ShotTarget;
    private float T = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.HasTheBall = false;
            IsBallFlying = true;
            T = 0;
        }
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.5f;
            float t01 = T / duration;
            Vector3 A = BallPosition;
            Vector3 B = ShotTarget; 
            Vector3 pos = Vector3.Lerp(A, B, t01);
        }
    }
}
