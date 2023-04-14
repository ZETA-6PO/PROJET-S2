using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    [SerializeField] private Image soundOnIcon;
    [SerializeField] private Image soundOffIcon;

    private bool _muted = false;

    [SerializeField] private bool toggleMusic, toggleEffects;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))//si c'est pas mute 
        {
            PlayerPrefs.SetInt("muted", 0);
        }
        Load();
        UpdateIcon();
        Mute();
        Debug.Log("Started");
        
    }

    public void OnButtonPress()
    {
        Debug.Log("OnButtonPress called");
        _muted = !_muted;
        Mute();
        Save();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        soundOnIcon.enabled = !_muted;
        soundOffIcon.enabled = _muted;
    }

    private void Load()
    {
        _muted = PlayerPrefs.GetInt("muted") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("muted",_muted ? 1 : 0);
    }

    private void Mute()
    {
        Debug.Log("Mute called");
        if (toggleEffects) SoundManager.Instance.MuteEffect(_muted);
        if (toggleMusic) SoundManager.Instance.MuteMusic(_muted);
    }
    
}
