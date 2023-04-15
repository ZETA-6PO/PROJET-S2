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
    public TMP_Text coinText;
    

    public void UpdateInventory(Dictionary<Item, int> items)
    {
        foreach (Cell cell in panel.GetComponents<Cell>())
        {
            Destroy(cell);
        }

        foreach (Item item in items.Keys)
        {
            Cell cell = GetCell(item);
            if (cell is null)
            {
                NewCell(item, items[item]);
            }
        }
    }

    public Cell GetCell(Item item)
    {
        foreach (Cell cell in panel.GetComponents<Cell>())
        {
            if (cell.item.Equals(item))
            {
                return cell;
            }
        }

        return null;
    }

    public void NewCell(Item item, int number)
    {
        GameObject cellObject = Instantiate(cellPrefab, panel.transform);
        Cell cell = cellObject.GetComponent<Cell>();
        cell.InitialiseCell(item, number, this);
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
