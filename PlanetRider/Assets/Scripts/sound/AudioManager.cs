using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip AudioClip;

    [Range(0.0f, 1.0f)]
    public float volume;

    [Range(0.1f, 3.0f)]
    public float pitch;

    [HideInInspector]
    public AudioSource AudioSource;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private float musicVolume;
    private bool musicTweening = false;

    public void SetFxVolume(float value)
    {
        PlayerPrefs.SetFloat("fxVolume", value);
        foreach(var sound in sounds)
        {
            sound.AudioSource.volume = value;
        }
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("musicVolume", value);
        musicVolume = value;
        if(musicTweening)
        {
            LeanTween.cancel(gameObject);
        }
        music.volume = value;
    }

    public AudioSource music;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);


        foreach (var sound in sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.AudioClip;
            sound.AudioSource.volume = sound.volume;
            sound.AudioSource.pitch = sound.pitch;
        }
    }

    private void Start()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        SetFxVolume(PlayerPrefs.GetFloat("fxVolume"));
        FadeInMusic();
    }

    public void Play(string soundName)
    {
        Sound sound = Array.Find(sounds, (s) => { return s.name == soundName; });
        sound.AudioSource.Play();
    }

    public void FadeOutMusic()
    {
        LeanTween.cancel(gameObject);
        musicTweening = true;
        LeanTween.value(gameObject, musicVolume, 0.0f, 5.0f).setOnUpdate((value) =>
        {
            music.volume = value;
        }).setOnComplete(() =>
        {
            music.Stop();
            musicTweening = false;
        });
    }

    public void FadeInMusic()
    {
        LeanTween.cancel(gameObject);
        music.Play();
        musicTweening = true;
        LeanTween.value(gameObject, 0.0f, musicVolume, 5.0f).setOnUpdate((value) =>
        {
            music.volume = value;
        }).setOnComplete(()=> { musicTweening = false; });
    }

}
