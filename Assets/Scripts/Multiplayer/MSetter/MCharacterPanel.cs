using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MCharacterPanel : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TMP_Text fighterName;
    [SerializeField] private TMP_Text fighterSurname;
    [SerializeField] private TMP_Text resist;
    [SerializeField] private TMP_Text inspi;


    private MSetter _launcher;

    public void Initialize(MSetter launcher)
    {
        _launcher = launcher;
    }


    public void Change(MultiplayerFighter fighter)
    {
        img.sprite = fighter.image;
        fighterName.text = fighter.nickName;
        fighterSurname.text = fighter.surname;
        resist.text = fighter.maxResistance.ToString();
        inspi.text = fighter.maxInspiration.ToString();
    }

    public void OnClickSelect()
    {
        _launcher.TakeFighter();
    }
}
