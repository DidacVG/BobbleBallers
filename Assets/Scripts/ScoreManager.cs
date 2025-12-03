using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int scoreA = 0;
    public int scoreB = 0;

    public Text scoreAText;
    public Text scoreBText;

    private void Awake()
    {
        Instance = this;
    }

    public void AddPoints(bool isTeamA, int points)
    {
        if (isTeamA)
            scoreA += points;
        else
            scoreB += points;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreAText != null)
            scoreAText.text = scoreA.ToString();

        if (scoreBText != null)
            scoreBText.text = scoreB.ToString();
    }
}
