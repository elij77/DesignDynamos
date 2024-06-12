
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Animator animator;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider masterSlider;

    public sound[] musicSounds, sfxSounds;
    public sound[] audJump;
    public sound[] audhit;
    public AudioMixer masterMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //DontDestroyOnLoad(Instance);
        }else
        {
            Destroy(gameObject);
        }
        
        
        
    }

    private void Start()
    {
        

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            load();
        }
        else
        {
            SetMusicVolume();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            playMusic("Main Menu");
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playMusic("Level 1");
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
