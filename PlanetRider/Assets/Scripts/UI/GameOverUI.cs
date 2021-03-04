using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject container;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;

    private void Start()
    {
        container.transform.localScale = Vector3.zero;
        LeanTween.scale(container, new Vector3(1.0f, 0.01f, 1.0f), 0.2f);
        LeanTween.scale(container, Vector3.one, 0.2f).setDelay(0.2f);
    }
    public void PlayAgainPressed()
    {
        LeanTween.scale(container, new Vector3(1.0f, 0.01f, 1.0f), 0.2f);
        LeanTween.scale(container, Vector3.zero, 0.2f).setDelay(0.2f).setOnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void ExitToMenuPressed()
    {
        AudioManager.instance.FadeInMusic();
        LeanTween.scale(container, new Vector3(1.0f, 0.01f, 1.0f), 0.2f);
        LeanTween.scale(container, Vector3.zero, 0.2f).setDelay(0.2f).setOnComplete(() =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });
    }
}
