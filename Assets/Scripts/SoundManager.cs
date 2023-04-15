using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource musicSource, effectsSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (effectsSource is not null) effectsSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void MuteEffect(bool muted)
    {
        Debug.Log("MuteEffect called");
        if (effectsSource is not null)effectsSource.mute = muted;
    }

    public void MuteMusic(bool muted)
    {
        Debug.Log("MuteMusic called");
        if (musicSource is not null)musicSource.mute = muted;
        
    }
}
