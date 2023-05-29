using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MAttackMenu : MonoBehaviour
{
    private UnityAction<AttackObject> OnAttackSelected;
    public GameObject cellPrefab;
    private List<MAttackCell> cells = new List<MAttackCell>();
    public GameObject panel;
    private CombatManager _system;
    public DescriptionSpace space;


    public void OpenMenu(UnityAction<AttackObject> onAttackSelected, CombatManager system)
    {
        Debug.Log("AttackMenuOpened");
        _system = system;
        gameObject.SetActive(true);
        OnAttackSelected = onAttackSelected;
        foreach (var c in cells)
        {
            Destroy(c.gameObject);
        }
        cells.Clear();
        Updt();
        space.Change(_system.localAttackUsage.Keys.ToList()[0]);
        
    }

    public void Updt()
    {
        if (_system.localAttackUsage is not null)
        {
            int i = 0;
            foreach (var kv in _system.localAttackUsage)
            {
                Debug.Log($"StartAttackMenu() -> localAttackUsage at {i} -> attack {kv.Key.name}, use : {kv.Value}");
                NewCell(kv.Key, kv.Value, i + 1);
                i += 1;
            }
        }
        else
        {
            Debug.Log("StartAttackMenu() -> localAttackUsage is null");
        }
    }

    private void NewCell(AttackObject item, int number,int index)
    {
        GameObject cellObject = Instantiate(cellPrefab, panel.transform);
        MAttackCell attackCell = cellObject.GetComponent<MAttackCell>();
        cells.Add(attackCell);
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
    public void CloseMenu()
    {
        gameObject.SetActive(false);
        OnAttackSelected(null);
    }
    
    public void OnclickAttack(int i)
    {
        Debug.Log($"OnClickAttack() -> {_system.localAttackUsage.Keys.ToArray()[i-1].name}");
        OnAttackSelected(_system.localAttackUsage.Keys.ToArray()[i-1]);
        CloseMenu();
    }
    
}