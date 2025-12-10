using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public Tiro tiro;
    public Transform BallPosition;
    public Transform BallTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Posesion(Tiro HasTheBall)
    {
        if (HasTheBall) 
        {
            BallTransform.position = BallPosition.position;
        }
    }

}
