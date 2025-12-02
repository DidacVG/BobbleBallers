using UnityEngine;

public class BarraTiro : MonoBehaviour
{
    public RectTransform arrow;

    [Header("Velocidad base y dinámica")]
    public float baseSpeed = 400f;
    private float currentSpeed;
    private bool movingUp = true;

    public float minY = -200f;
    public float maxY = 200f;

    [Header("Perfect Zone")]
    public float perfectMinY = -20f;
    public float perfectMaxY = 20f;

    // --- NUEVO: referencias dinámicas ---
    [HideInInspector] public Transform player;
    [HideInInspector] public Transform rival;
    public float detectionRange = 2f;
    public float speedMultiplier = 2f;

    void Update()
    {
        if (player != null && rival != null)
            UpdateSpeedDependingOnRival();
        else
            currentSpeed = baseSpeed;

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
            currentSpeed = baseSpeed * speedMultiplier;
        else
            currentSpeed = baseSpeed;
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
