using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject panel;
    public Dictionary<Item,int> items = new Dictionary<Item, int>();
    public DescriptionSpace space;
    public List<Item> itemBase;
    public int coinCount;
    public TMP_Text coinText;
    
    public List<Cell> cells => panel.GetComponentsInChildren<Cell>().ToList();
    
    public Cell GetCell(Item item)
    {   
        Debug.Log("Get Cell called");
        Debug.Log("cells count = "+cells.Count);
        foreach (Cell cell in cells)
        {
            Debug.Log(cell.item.name);
            if (cell.item.Equals(item))
            {
                return cell;
            }
        }
        return null;
    }
    
    public void AddOneItem(Item item)
    {
        if (items.Keys.Contains(item))
        {
            if (item.consumable)
            {
                items[item] += 1;
                GetCell(item).Update();
            }
           
        }
        else
        {
            items[item] = 1;
            GameObject cellObject = Instantiate(cellPrefab, panel.transform);
            Cell cell = cellObject.GetComponent<Cell>();
            cell.InitialiseCell(item,this);
        }
    }

    public void RemoveItem(Item item)
    {
        if (items[item] > 0)
        {
            if (item.consumable)
            {
                items[item] -= 1;
            }
        }
        if (!items.Keys.Contains(item))
        {
            Debug.Log("it not contains");
        }
    }

    public void AddItems(List<Item> list)
    {
        foreach (Item item in list)
        {
            AddOneItem(item);
        }
    }
    
    public void TestAddButton()
    {
        AddItems(itemBase);
    }

    public void AddCoins(int n)
    {
        coinCount += n;
        UpdateCoin();
    }

    public void RemoveCoins(int n)
    {
        coinCount -= n;
        UpdateCoin();
    }
    public void UpdateCoin()
    {
        coinText.text = coinCount.ToString();
    }
}
