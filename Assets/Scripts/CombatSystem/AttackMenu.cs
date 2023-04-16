using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackMenu : MonoBehaviour
{
    private Action<AttackObject> OnAttackSelected;
    private AttackObject[] AttackList;
    public GameObject cellPrefab;
    private List<LaunchAttackCell> cells;
    public GameObject panel;
    private BattleSystem _system;


    public void OpenMenu(Fighter player,Action<AttackObject> onAttackSelected,BattleSystem system)
    {
        AttackList = player.Attacks;
        _system = system;
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
                    NewCell(attack, _system._uses[i],i+1);
                }
            }
        }
    }
    
    private void NewCell(AttackObject item, int number,int index)
    {
        GameObject cellObject = Instantiate(cellPrefab, panel.transform);
        LaunchAttackCell attackCell = cellObject.GetComponent<LaunchAttackCell>();
        attackCell.Initialise(item,this,number,index);
    }

    private int SetMaxUse(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return 6;
            case Rarity.Hyped:
                return 4;
            case Rarity.Legendary:
                return 2;
            default:
                return 6;
        }
    }
    private void CloseMenu()
    {
        gameObject.SetActive(false);
        OnAttackSelected(null);
    }
    
    public void OnclickAttack(int i)
    {
        Debug.Log("OnclickAttack");
        _system._uses[i - 1] -= 1;
        OnAttackSelected(AttackList[i-1]);
        CloseMenu();
    }
    
}
