
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;

    public sound[] musicSounds, sfxSounds;
    public AudioMixer masterMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        playMusic("Main Menu");

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            load();
        }
        else
        {
            SetMusicVolume();
        }
        
    }

    public void playMusic(string name)
    {
        sound Sound = Array.Find(musicSounds, x => x.soundName == name);

        musicSource.clip = Sound.clip;

        musicSource.Play();
    }

    public void playSFX(string name)
    {
        sound Sound = Array.Find(sfxSounds, x => x.soundName == name);

        SFXSource.clip = Sound.clip;

        SFXSource.Play();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        //Debug.Log(volume);
        masterMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        //Debug.Log(volume);
        masterMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        //Debug.Log(volume);
        masterMixer.SetFloat("maxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("maxVolume", volume);
    }

    private void load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        masterSlider.value = PlayerPrefs.GetFloat("maxVolume");

        SetMusicVolume();
        SetSFXVolume();
        SetMasterVolume();
    }
    
}
