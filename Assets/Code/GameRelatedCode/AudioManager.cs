using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Cargar valores guardados o usar por defecto
        musicSlider.value = PlayerPrefs.GetFloat("VolumeMusic", 0.45f);
        sfxSlider.value = PlayerPrefs.GetFloat("VolumeSFX", 0.45f);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("VolumeMusic", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20);
        PlayerPrefs.SetFloat("VolumeMusic", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("VolumeSFX", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1)) * 20);
        PlayerPrefs.SetFloat("VolumeSFX", volume);
    }
}
