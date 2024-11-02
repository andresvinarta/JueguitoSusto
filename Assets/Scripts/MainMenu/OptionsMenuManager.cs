using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [SerializeField]
    private Slider MasterSlider, MusicSlider, SoundEffectsSlider;

    [SerializeField]
    private AudioMixer MainMixer;


    public void SetMasterVolume(float NewVolume)
    {
        MainMixer.SetFloat("MasterVolume", NewVolume);
    }

    public void SetMusicVolume(float NewVolume)
    {
        MainMixer.SetFloat("MusicVolume", NewVolume);
    }

    public void SetSFXVolume(float NewVolume)
    {
        MainMixer.SetFloat("SFXVolume", NewVolume);
    }
}
