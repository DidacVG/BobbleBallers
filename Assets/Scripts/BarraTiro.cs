using UnityEngine;
using UnityEngine.UI;

public class BarraTiro : MonoBehaviour
{
    public RectTransform arrow;
    public Image arrowImage;

    [Header("Velocidad de movimiento")]
    public float baseSpeed = 400f;
    private float currentSpeed;
    private bool movingUp = true;

    public float minY = -200f;
    public float maxY = 200f;

    [Header("Perfect Zone")]
    public float perfectMinY = -20f;
    public float perfectMaxY = 20f;

    [Header("Rivales cercanos")]
    public Transform player;
    public Transform[] rivals;
    public float detectionRange = 2f;

    [Tooltip("Cuánto baja la fuerza por cada rival (0.2 = -20%)")]
    public float rivalPenalty = 0.2f;

    public float ForceMultiplier { get; private set; } = 1f;
    private float powerLimit = 1f;

    public float speedMultiplier = 1.3f;

    private float slowMultiplier = 1f;
    private Coroutine slowRoutine;


    void Update()
    {
        UpdateRivalEffects();

        float finalSpeed = currentSpeed * slowMultiplier;

        float newY = arrow.anchoredPosition.y + (movingUp ? 1 : -1) * finalSpeed * Time.deltaTime;

        float maxVisualY = minY + (maxY - minY) * powerLimit;

        if (newY > maxVisualY)
        {
            newY = maxVisualY;
            movingUp = false;
        }
        if (newY < minY)
        {
            newY = minY;
            movingUp = true;
        }

        arrow.anchoredPosition = new Vector2(arrow.anchoredPosition.x, newY);
    }


    // ================================================================
    //       DETECTAR RIVALES – REDUCIR POTENCIA + CAMBIAR COLOR
    // ================================================================
    void UpdateRivalEffects()
    {
        if (player == null || rivals == null || rivals.Length == 0)
        {
            currentSpeed = baseSpeed;
            ForceMultiplier = 1f;
            powerLimit = 1f;
            SetArrowColor(0);
            return;
        }

        int closeCount = 0;

        for (int i = 0; i < rivals.Length; i++)
        {
            if (rivals[i] == null) continue;

            if (Vector3.Distance(player.position, rivals[i].position) < detectionRange)
                closeCount++;
        }

        // Velocidad base (si quieres seguir usándola)
        currentSpeed = baseSpeed * Mathf.Pow(speedMultiplier, closeCount);

        // Reducción de fuerza del tiro
        ForceMultiplier = 1f - rivalPenalty * closeCount;
        ForceMultiplier = Mathf.Clamp(ForceMultiplier, 0.3f, 1f);

        // La barra solo sube hasta este límite
        powerLimit = ForceMultiplier;

        // 🔥 CAMBIAR COLOR
        SetArrowColor(closeCount);
    }


    // ================================================================
    //                        COLORES DE LA BARRA
    // ================================================================
    void SetArrowColor(int rivals)
    {
        if (arrowImage == null) return;

        switch (rivals)
        {
            case 0:
                arrowImage.color = Color.green;
                break;
            case 1:
                arrowImage.color = Color.yellow;
                break;
            case 2:
                arrowImage.color = new Color(1f, 0.5f, 0f); // naranja
                break;
            default:
                arrowImage.color = Color.red;
                break;
        }
    }


    // ================================================================
    //              FUNCIONES QUE USAN OTROS SCRIPTS
    // ================================================================
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