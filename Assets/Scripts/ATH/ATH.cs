using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ATH : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text timeText;
    Time_Gestion dnScript;

    public void Start()
    {
        dnScript = FindObjectOfType<Time_Gestion>();
        if (dnScript is not null)
            dnScript.onUpdateMinute.AddListener(UpdateTime);
    }

    // Update is called once per frame
    public void UpdateCoin()
    {
        coinText.text = GameManager.Instance.coin.ToString();
    }

    public void UpdateTime(int hours)
    {
        timeText.text = string.Format("{0:00}", hours) + " h";
    }


    public void EnableATH()
    {
        gameObject.SetActive(true);
        UpdateCoin();
    }

    public void DisableATH()
    {
        gameObject.SetActive(false);
    }

    public void ButtonPlayerMenu()
    {
        GameManager.Instance.OpenInventory();
    }
    
    
}
