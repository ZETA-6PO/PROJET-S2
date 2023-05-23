using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealMenu : MonoBehaviour
{
    
    public GameObject panel;
    public GameObject healCellPrefab;
    private UnityAction<Consumable> onHealSelected;
    public DescriptionSpace space;
    

    public void OpenMenu(UnityAction<Consumable> callBack)
    {
        gameObject.SetActive(true);
        onHealSelected = callBack;
        space.Change(GameManager.Instance.heal.Keys.First());
    }

    public void Start()
    {
        if (GameManager.Instance is not null && GameManager.Instance.items is not null)
        {
            foreach (Consumable item in GameManager.Instance.heal.Keys)
            {
                NewCell(item, GameManager.Instance.heal[item]);
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
