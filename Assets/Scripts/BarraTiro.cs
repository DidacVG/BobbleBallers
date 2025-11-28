using UnityEngine;

public class BarraTiro : MonoBehaviour
{
    public RectTransform arrow;
    public float baseSpeed = 400f;       // velocidad normal
    private float currentSpeed;          // velocidad que realmente usa la barra
    private bool movingUp = true;

    public float minY = -200f;
    public float maxY = 200f;

    [Header("Perfect Zone")]
    public float perfectMinY = -20f;
    public float perfectMaxY = 20f;

    [Header("Defensa")]
    public Transform player;             // tu jugador
    public Transform rival;              // el defensor
    public float detectionRange = 2f;    // distancia a la que te afecta
    public float speedMultiplier = 2f;   // cuánta velocidad extra cuando te cubren

    void Update()
    {
        UpdateSpeedDependingOnRival();

        float newY = arrow.anchoredPosition.y + (movingUp ? 1 : -1) * currentSpeed * Time.deltaTime;

        if (newY > maxY)
        {
            newY = maxY;
            movingUp = false;
        }
        if (newY < minY)
        {
            newY = minY;
            movingUp = true;
        }

        arrow.anchoredPosition = new Vector2(arrow.anchoredPosition.x, newY);
    }

    void UpdateSpeedDependingOnRival()
    {
        float dist = Vector3.Distance(player.position, rival.position);

        if (dist < detectionRange)
        {
            currentSpeed = baseSpeed * speedMultiplier;   // MÁS DIFÍCIL SI TE CUBREN
        }
        else
        {
            currentSpeed = baseSpeed;                     // velocidad normal
        }
    }

    public float GetPower()
    {
        return (arrow.anchoredPosition.y - minY) / (maxY - minY);
    }

    public bool IsPerfect()
    {
        float y = arrow.anchoredPosition.y;
        return y >= perfectMinY && y <= perfectMaxY;
    }
}
