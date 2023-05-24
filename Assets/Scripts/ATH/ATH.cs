using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ATH : MonoBehaviour
{
    public TMP_Text coinText;
    
    // Update is called once per frame
    public void UpdateCoin()
    {
        coinText.text = GameManager.Instance.coin.ToString();
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
