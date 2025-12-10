using UnityEngine;
using System.Collections;

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

    // --- AHORA MÚLTIPLES RIVALES ---
    [Header("Rivales cercanos")]
    public Transform player;
    public Transform[] rivals;
    public float detectionRange = 2f;

    [Tooltip("Cuánto aumenta la velocidad por cada rival cercano")]
    public float speedMultiplier = 1.3f;

    // --- Potenciador temporal (como Mario Kart) ---
    private float slowMultiplier = 1f;
    private Coroutine slowRoutine;

    void Update()
    {
        UpdateBaseSpeedFromRivals();

        float finalSpeed = currentSpeed * slowMultiplier;

        float newY = arrow.anchoredPosition.y + (movingUp ? 1 : -1) * finalSpeed * Time.deltaTime;

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

    // =================================================================
    //    DETECTAR varios rivales y ACUMULAR multiplicadores
    // =================================================================
    void UpdateBaseSpeedFromRivals()
    {
        if (player == null || rivals == null || rivals.Length == 0)
        {
            currentSpeed = baseSpeed;
            return;
        }

        int closeCount = 0;

        for (int i = 0; i < rivals.Length; i++)
        {
            if (rivals[i] == null) continue;

            if (Vector3.Distance(player.position, rivals[i].position) < detectionRange)
                closeCount++;
        }

        if (closeCount == 0)
        {
            currentSpeed = baseSpeed;
            return;
        }

        // velocidad = baseSpeed * (speedMultiplier ^ closeCount)
        currentSpeed = baseSpeed * Mathf.Pow(speedMultiplier, closeCount);
    }

    // =================================================================
    //  POTENCIADOR TEMPORAL (efecto que dura X segundos)
    // =================================================================
    public void ApplyTemporarySlow(float multiplier, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowEffect(multiplier, duration));
    }

    IEnumerator SlowEffect(float multiplier, float duration)
    {
        slowMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        slowMultiplier = 1f;
        slowRoutine = null;
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