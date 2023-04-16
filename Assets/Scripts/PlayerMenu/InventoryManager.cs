using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject panel;
    public DescriptionSpace space;
    

    public void UpdateInventory(Dictionary<Item, int> items)
    {
        foreach (Item item in items.Keys)
        {
            int number = items[item];
            if (number>0) NewCell(item,number);
        }
    }

    
    public void NewCell(Item item, int number)
    {
        GameObject cellObject = Instantiate(cellPrefab, panel.transform);
        Cell cell = cellObject.GetComponent<Cell>();
        cell.InitialiseCell(item, number, this);
    }

    
}
