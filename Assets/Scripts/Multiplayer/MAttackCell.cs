using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MAttackCell : MonoBehaviour
{
    //UI objects
    public Image icon;
    public TMP_Text title;
    public TMP_Text countText;
    
    //
    private AttackObject _attack;
    private MAttackMenu _launcher;
    private int _number;
    private int _index;

    public void Initialise(AttackObject a, MAttackMenu m, int use, int i)
    {
        Debug.Log($"InitialiseCell() -> {a.name}, index: {i}");
        _index = i;
        _attack = a;
        _launcher = m;
        icon.sprite = _attack.image;
        title.text = _attack.name;
        _number = _attack.MaxUse-use;
        if (_number < 0) _number = 0;
        countText.text = _number.ToString();
        enabled = true;
        ActiveCell();
    }
    
    private void ActiveCell()
    {
        icon.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        countText.gameObject.SetActive(true);
    }


    public void CellClicked()
    {
        _launcher.space.Change(_attack);
    }

    public void PlayClicked()
    {
        if (_number>0)
        {
            countText.text = _number.ToString();
            _launcher.OnclickAttack(_index);
        }
    }
}
