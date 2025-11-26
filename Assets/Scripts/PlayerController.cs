using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform Ball;
    public Transform HandPosition;
    public Transform BallDetector;
    public bool HasTheBall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasTheBall)
        {
            Ball.position = HandPosition.position;
        }
    }
}
