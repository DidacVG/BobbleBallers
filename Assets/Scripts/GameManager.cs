using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Jugador seleccionado actualmente")]
    public MoverPersonajes selectedPlayer;

    [Header("Jugador que tiene la pelota")]
    public MoverPersonajes ballHolder;

    [Header("Pelota en escena")]
    public Transform ball;
    public Rigidbody ballRb;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Seleccionar jugador desde PlayerManager
    public void SetSelectedPlayer(MoverPersonajes player)
    {
        selectedPlayer = player;
    }

    // Dar la pelota a un jugador
    public void GiveBallTo(MoverPersonajes player)
    {
        ballHolder = player;
        player.HasTheBall = true;

        // Fijar pelota al jugador
        ball.SetParent(player.transform);
        ball.localPosition = new Vector3(0, 1f, 0.5f);

        ballRb.isKinematic = true;
        ballRb.linearVelocity = Vector3.zero;
    }

    // Quitar pelota del jugador (tiro o pase)
    public void RemoveBallFromPlayer()
    {
        if (ballHolder != null)
            ballHolder.HasTheBall = false;

        ballHolder = null;

        ball.SetParent(null);
        ballRb.isKinematic = false;
    }
}