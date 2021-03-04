using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerRocketPrefab;
    GameObject currentPlayerRocket;
    float currentScore;

    public GameObject gameOverUIPrefab;
    public LevelGenerator LevelGenerator;

    public event Action<float> ScoreChanged;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        
    }

    void ResetGame()
    {
        currentPlayerRocket = Instantiate(PlayerRocketPrefab);
        currentPlayerRocket.GetComponent<PlayerRocket>().speed = 5;
        currentPlayerRocket.GetComponent<PlayerRocket>().PlayerOutOfBounds += PlayerOutOfBounds;
        currentPlayerRocket.GetComponent<PlayerRocket>().PlayerLanded += UpdateScore;

        Camera.main.transform.position = new Vector3(0.0f, 4.25f, -10f);
        Camera.main.GetComponent<FollowCamera>().PlayerToFollow = currentPlayerRocket.GetComponent<PlayerRocket>();

        LevelGenerator.TargetPlayer = currentPlayerRocket.GetComponent<PlayerRocket>();
        AudioManager.instance.Play("start");
        AudioManager.instance.FadeOutMusic();
    }

    private void UpdateScore(Vector3 landedPlanetPosition, float landAngle)
    {
        float bonusMultiplier = Mathf.Abs((Mathf.Abs(landAngle) / 90.0f) - 1);

        currentScore += 1 + bonusMultiplier;

        ScoreChanged?.Invoke(currentScore);
    }

    private void PlayerOutOfBounds()
    {
        Destroy(currentPlayerRocket);

        GameObject uiPopup = Instantiate(gameOverUIPrefab);

        if (!PlayerPrefs.HasKey("highscore")) PlayerPrefs.SetFloat("highscore", 0.0f);

        float currentHighScore = PlayerPrefs.GetFloat("highscore");
        if(currentScore > currentHighScore)
        {
            PlayerPrefs.SetFloat("highscore", currentScore);
        }

        uiPopup.GetComponent<GameOverUI>().currentScoreText.text = "Current score\n" + currentScore.ToString("0.00");
        uiPopup.GetComponent<GameOverUI>().highScoreText.text = "High score\n" + PlayerPrefs.GetFloat("highscore").ToString("0.00");

        AudioManager.instance.Play("fail");
    }
}
