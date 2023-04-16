using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ATH : MonoBehaviour
{
    public TMP_Text coinText;

    // Update is called once per frame
    void Update()
    {
        coinText.text = GameManager.Instance.coin.ToString();
    }
}
