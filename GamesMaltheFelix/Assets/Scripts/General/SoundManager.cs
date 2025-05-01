using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    
    [SerializeField] private AudioMixer audioMixer; // Reference to the AudioMixer
    
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }



    public void SetMasterVolume(float level)
    {
        //audioMixer.SetFloat("masterVolume", level);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f); // Convert linear volume to decibels
    }


    public void SetSFXVolume(float level)
    {
        //audioMixer.SetFloat("sfxVolume", level);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f); // Convert linear volume to decibels
    }


    public void SetMusicVolume(float level)
    {
        //audioMixer.SetFloat("musicVolume", level);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f); // Convert linear volume to decibels
    }


}
