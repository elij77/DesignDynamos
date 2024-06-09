
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public sound[] musicSounds, sfxSounds;
    public AudioMixer masterMixer;

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
        
    }

    public void playMusic()
    {
        sound Sound = Array.Find(musicSounds, x => x.soundName == name);

        musicSource.clip = Sound.clip;

        musicSource.Play();
    }

    public void playSFX()
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

    private void load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
    
}
