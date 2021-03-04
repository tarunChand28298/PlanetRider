using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject container;

    public RectTransform parentPanel;

    [SerializeField] Slider fxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider difficultySlider;

    [SerializeField] Toggle bannerToggle;
    [SerializeField] Toggle interstetialToggle;


    private void Start()
    {
        Initialize();

        container.transform.localScale = Vector3.zero;
        LeanTween.scale(container, new Vector3(1.0f, 0.01f, 1.0f), 0.2f);
        LeanTween.scale(container, Vector3.one, 0.2f).setDelay(0.2f);
    }

    public void PlayButtonClicked()
    {
        LeanTween.scale(container, new Vector3(1.0f, 0.01f, 1.0f), 0.2f);
        LeanTween.scale(container, Vector3.zero, 0.2f).setDelay(0.2f).setOnComplete(() =>
        {
            SceneManager.LoadScene("MainGameScene");
        });
    }

    public void AboutButtonClicked()
    {
        LeanTween.value(gameObject, 0, 1080, 0.2f).setOnUpdate((value) =>
        {
            parentPanel.anchoredPosition = new Vector2(value, parentPanel.anchoredPosition.y);
        });
    }

    public void SettingsButtonClicked()
    {
        LeanTween.value(gameObject, 0, -1080, 0.2f).setOnUpdate((value) =>
        {
            parentPanel.anchoredPosition = new Vector2(value, parentPanel.anchoredPosition.y);
        });
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        LeanTween.value(gameObject, parentPanel.anchoredPosition.x, 0, 0.2f).setOnUpdate((value) =>
        {
            parentPanel.anchoredPosition = new Vector2(value, parentPanel.anchoredPosition.y);
        });
    }

    public void MusicSliderUpdate(float value)
    {
        AudioManager.instance.SetMusicVolume(value);
    }

    public void FxSliderUpdate(float value)
    {
        AudioManager.instance.SetFxVolume(value);
    }

    public void SetDifficulty(float value)
    {
        PlayerPrefs.SetFloat("difficulty", value);
    }

    public void SetBanner(bool value)
    {
        PlayerPrefs.SetString("banner", value.ToString());
    }

    public void SetInterstetial(bool value)
    {
        PlayerPrefs.SetString("interstetial", value.ToString());
    }

    void Initialize()
    {
        if (!PlayerPrefs.HasKey("difficulty")) PlayerPrefs.SetFloat("difficulty", 100);
        if (!PlayerPrefs.HasKey("musicVolume")) PlayerPrefs.SetFloat("musicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("fxVolume")) PlayerPrefs.SetFloat("fxVolume", 0.5f);
        if (!PlayerPrefs.HasKey("banner")) PlayerPrefs.SetString("banner", true.ToString());
        if (!PlayerPrefs.HasKey("interstetial")) PlayerPrefs.SetString("interstetial", true.ToString());

        fxSlider.value = PlayerPrefs.GetFloat("fxVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        difficultySlider.value = PlayerPrefs.GetFloat("difficulty");

        bannerToggle.isOn = bool.Parse(PlayerPrefs.GetString("banner"));
        interstetialToggle.isOn = bool.Parse(PlayerPrefs.GetString("interstetial"));
    }
}
