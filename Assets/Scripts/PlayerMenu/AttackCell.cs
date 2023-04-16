using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackCell : MonoBehaviour
{
    public Image attackIcon;
    public TMP_Text attackName;
    public AttackSelector selector;
    public AttackObject attack;

    public void InitialiseCell(AttackObject attack,AttackSelector manager)
    {
        this.attack = attack;
        selector = manager;
        attackIcon.sprite = attack.image;
        attackName.text = attack.name;
    }

    public void CellClicked()
    {
        selector.Select(attack);
    }
}
