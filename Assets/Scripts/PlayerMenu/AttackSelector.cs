using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AttackSelector : MonoBehaviour
{
    public GameObject attackCellPrefab;
    public GameObject panel;
    private int index;
    private StatsManager manager;

    public void Initialise(List<Item> toSelect,StatsManager m ,int i)
    {
        manager = m;
        index = i;
        foreach (Item item in toSelect)
        {
            GameObject cellObject = Instantiate(attackCellPrefab, panel.transform);
            AttackCell cell = cellObject.GetComponent<AttackCell>();
            cell.InitialiseCell(item,this);
        }
    }

    public void Select(Item item)
    {
        manager.ChangeAttack(index,item);
    }
    
}
