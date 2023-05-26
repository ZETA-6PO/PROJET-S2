using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAttackMenu : MonoBehaviour
{
    private Action<AttackObject> OnAttackSelected;
    private AttackObject[] AttackList;
    public GameObject cellPrefab;
    private List<LaunchAttackCell> cells;
    public GameObject panel;
    private  int[] _uses;


    public void OpenMenu(MultiplayerFighter player,Action<AttackObject> onAttackSelected)
    {
        Debug.Log("AttackMenuOpened");
        AttackList = player.attacks;
        _uses = player.attacksUsage;
        gameObject.SetActive(true);
        OnAttackSelected = onAttackSelected;
    }
    
    public void Start()
    {
        if (AttackList is not null)
        {
            for (int i = 0; i < 4; i++)
            {
            
                AttackObject attack = AttackList[i];
                if (attack is not null)
                {
                    NewCell(attack, _uses[i],i+1);
                }
            }
        }
    }
    
    private void NewCell(AttackObject item, int number,int index)
    {
        GameObject cellObject = Instantiate(cellPrefab, panel.transform);
        MAttackCell attackCell = cellObject.GetComponent<MAttackCell>();
        attackCell.Initialise(item,this,number,index);
    }
    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void OnClickCloseMenu()
    {
        gameObject.SetActive(false);
        OnAttackSelected(null);
    }

    public void OnclickAttack(int i)
    {
        OnAttackSelected(AttackList[i-1]);
        CloseMenu();
    }
}
