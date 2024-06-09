using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private Slider volumeSlider;

    public AudioMixer audioMixer;
    public AudioClip background;


    public void SetVolume ()
    {
        float volume = volumeSlider.value;
        //Debug.Log(volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }
    private void Start()
    {
        SetVolume();
        musicSource.clip = background;
        musicSource.Play();
    }
}
