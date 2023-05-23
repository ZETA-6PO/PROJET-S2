using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource musicSource, effectsSource;
    public AudioClip gameMusic;

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

    public void StopSound()
    {
        if (effectsSource is not null) effectsSource.Stop();
    }
    
    public void PlayMusic()
    {
        if (musicSource is not null) musicSource.UnPause();
    }

    public void StopMusic()
    {
        if (musicSource is not null) musicSource.Pause();
    }

    public void PlayDefaultMusic()
    {
        if (musicSource is null) return;
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlayCertainMusic(AudioClip audio)
    {
        if (musicSource is null) return;
        musicSource.clip = audio;
        musicSource.loop = true;
        musicSource.Play();
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
