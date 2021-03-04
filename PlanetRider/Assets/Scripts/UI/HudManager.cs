using TMPro;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        gameManager.ScoreChanged += UpdateScoreInUI;
    }

    private void UpdateScoreInUI(float newScore)
    {
        scoreText.text = newScore.ToString("0.00");
    }
}
