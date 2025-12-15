using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI teamAText;
    public TextMeshProUGUI teamBText;

    void Awake()
    {
        Instance = this;
    }

    public void RefreshUI()
    {
        teamAText.text = GameManager.Instance.scoreTeamA.ToString();
        teamBText.text = GameManager.Instance.scoreTeamB.ToString();
    }
}