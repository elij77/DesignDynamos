
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] private Slider volumeSlider;

    public AudioMixer audioMixer;
    public AudioClip background;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            load();
        }
        else
        {
            SetVolume();
        }
        musicSource.clip = background;
        musicSource.Play();
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        //Debug.Log(volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    private void load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");

        SetVolume();
    }
    
}
