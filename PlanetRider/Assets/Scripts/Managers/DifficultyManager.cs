using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    GameManager gameManager;
    float currentScore;
    float ScoreToMaxDifficulty = 100;

    void Start()
    {
        ScoreToMaxDifficulty = PlayerPrefs.GetFloat("difficulty");

        gameManager = GetComponent<GameManager>();
        gameManager.ScoreChanged += UpdateInternalScore;
    }

    private void UpdateInternalScore(float newScore)
    {
        currentScore = newScore;
    }

    public float GetSpeedMultiplier()
    {
        return Mathf.Clamp(currentScore / ScoreToMaxDifficulty, 0, 1) * 5;
    }

    public float GetSizeMultiplier()
    {
        return Mathf.Clamp(currentScore / ScoreToMaxDifficulty, 0, 1) * 1;
    }

    public bool GetTimedProbability()
    {
        bool fiftyFifty = Random.Range(0, 2) == 0 ? false : true;
        return currentScore > (ScoreToMaxDifficulty - (0.2f * ScoreToMaxDifficulty) ) ? fiftyFifty : false;
    }
}
