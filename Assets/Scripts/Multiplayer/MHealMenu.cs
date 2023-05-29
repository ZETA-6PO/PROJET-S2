using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MHealMenu : MonoBehaviour
{
    
    public GameObject panel;
    public MHealCell healCellPrefab;
    public List<MHealCell> cells = new List<MHealCell>();
    private UnityAction<Consumable> onHealSelected;
    private Dictionary<Consumable, int> consumableDico;
    public DescriptionSpace space;
    

    public void OpenMenu(UnityAction<Consumable> callBack, Dictionary<Consumable, int> consumable)
    {
        gameObject.SetActive(true);
        onHealSelected = callBack;
        consumableDico = consumable;
        foreach (var c in cells)
        {
            Destroy(c.gameObject);
        }

        cells.Clear();
        Updt();
        space.Change(consumable.First((consumable1 => consumable1.Value > 0)).Key);
    }

    public void Updt()
    {
        int i = 0;
        foreach (var kv in consumableDico)
        {
            if (kv.Value > 0)
            {
                NewCell(kv.Key, kv.Value);
            }
            i += 1;
        }
    }

    private void NewCell(Consumable item, int number)
    {
        MHealCell cellObject = Instantiate(healCellPrefab, panel.transform);
        MHealCell healCell = cellObject.GetComponent<MHealCell>();
        cells.Add(healCell);
        healCell.Initialise(item,this,number);
    }


    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    
    public void OnclickConsumable(Consumable c)
    {
        onHealSelected(c);
        CloseMenu();
    }
}
