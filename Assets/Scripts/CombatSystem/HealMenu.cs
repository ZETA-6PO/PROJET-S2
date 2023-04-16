using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealMenu : MonoBehaviour
{
    
    public GameObject panel;
    public GameObject healCellPrefab;
    private List<HealCell> cellList;
    private UnityAction<Consumable> onHealSelected;
    

    public void OpenMenu(UnityAction<Consumable> callBack)
    {
        gameObject.SetActive(true);
        onHealSelected = callBack;
    }

    public void Start()
    {
        if (GameManager.Instance is not null && GameManager.Instance.items is not null)
        {
            Dictionary<Item, int> items = GameManager.Instance.items;
            foreach (Item item in items.Keys)
            {
                if (item is Consumable && items[item] > 0)
                {
                    NewCell((Consumable)item, items[item]);
                }
            }
        }
        
        
    }

    private void NewCell(Consumable item, int number)
    {
        GameObject cellObject = Instantiate(healCellPrefab, panel.transform);
        HealCell healCell = cellObject.GetComponent<HealCell>();
        healCell.Initialise(item,this,number);
    }


    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    
    public void OnclickConsumable(Consumable c)
    {
        onHealSelected(c);
        CloseMenu();
    }
}
