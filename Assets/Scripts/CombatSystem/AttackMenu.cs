using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    private Action<AttackObject> OnAttackSelected;
    private AttackObject[] AttackList;
    public Button Attack1;
    public Button Attack2;

    public void OpenMenu(Fighter player, Action<AttackObject> onAttackSelected)
    {
        gameObject.SetActive(true);
        OnAttackSelected = onAttackSelected;
        Initialize(player.Attacks);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    
    
    public void Initialize(AttackObject[] attacks)
    {
        AttackList = attacks;
        Attack1.tag = attacks[0].name;
        Attack2.tag = attacks[1].name;
    }

    public void OnClickAttack1()
    {
        OnAttackSelected(AttackList[0]);
        CloseMenu();
    }
    
    public void OnClickAttack2()
    {
        OnAttackSelected(AttackList[1]);
        CloseMenu();
    }



}
