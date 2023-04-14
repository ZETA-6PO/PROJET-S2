using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    private Action<AttackObject> OnAttackSelected;
    private AttackObject[] AttackList;
    public Button Attack1;
    public Button Attack2;
    public Button Attack3;
    public Button Attack4;

    public void OpenMenu(Fighter player, Action<AttackObject> onAttackSelected)
    {
        gameObject.SetActive(true);
        OnAttackSelected = onAttackSelected;
        AttackList = player.Attacks;
    }

    public void CloseMenu()
    {
        OnAttackSelected(null);
        gameObject.SetActive(false);
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
    public void OnClickAttack3()
    {
        OnAttackSelected(AttackList[2]);
        CloseMenu();
    }
    public void OnClickAttack4()
    {
        OnAttackSelected(AttackList[4]);
        CloseMenu();
    }



}
