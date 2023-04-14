using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<Item> itemsToBuy;
    public Inventory Inventory;
    public GameObject panel;
    public GameObject shopCellPrefab;
    public DescriptionSpace space;

    private void Start()
    {
        foreach (Item item in itemsToBuy)
        {
            GameObject cellObject = Instantiate(shopCellPrefab, panel.transform);
            ShopCell cell = cellObject.GetComponent<ShopCell>();
            cell.InitialiseCell(item,Inventory,this);
        }
    }

    public void Buy(Item item)
    {
        
        if (!item.consumable)
        {
            if (Inventory.items.ContainsKey(item))
            {
                return;
            } 
        }
        if (item.price <= Inventory.coinCount)
        {
            Inventory.RemoveCoins(item.price);
            Inventory.AddOneItem(item);
        }
    }
    
    

}
