using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text countText;
    //
    private Consumable _consumable;
    private HealMenu _launcher;
    private int number;

    public void Initialise(Consumable c, HealMenu m,int n)
    {
        number = n;
        _consumable = c;
        _launcher = m;
        icon.sprite = _consumable.image;
        title.text = _consumable.name;
        if (number < 0) number = 0;
        countText.text = number.ToString();
    }
    
    public void CellClicked()
    {
        if (number>0)
        {
            number -= 1;
            countText.text = number.ToString();
            _launcher.OnclickConsumable(_consumable);
        }
    }
}
