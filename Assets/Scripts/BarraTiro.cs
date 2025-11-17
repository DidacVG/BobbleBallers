using UnityEngine;

public class BarraTiro : MonoBehaviour
{
    public RectTransform arrow;
    public float speed = 400f;
    private bool movingUp = true;

    public float minY = -200f;
    public float maxY = 200f;

    [Header("Perfect Zone")]
    public float perfectMinY = -20f;
    public float perfectMaxY = 20f;

    void Update()
    {
        float newY = arrow.anchoredPosition.y + (movingUp ? 1 : -1) * speed * Time.deltaTime;

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

    // Devuelve un valor entre 0 y 1
    public float GetPower()
    {
        return (arrow.anchoredPosition.y - minY) / (maxY - minY);
    }

    // Saber si es tiro perfecto
    public bool IsPerfect()
    {
        float y = arrow.anchoredPosition.y;
        return y >= perfectMinY && y <= perfectMaxY;
    }
}
