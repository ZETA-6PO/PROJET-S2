using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject shopCellPrefab;
    public DescriptionSpace space;

    public void UdpdateShop(List<Item> toBuy)
    {
        foreach (Item item in toBuy)
        {
            GameObject cellObject = Instantiate(shopCellPrefab, panel.transform);
            ShopCell cell = cellObject.GetComponent<ShopCell>();
            cell.InitialiseCell(item,this);
        }
    }

    public void Buy(Item item)
    {
        if (!item.consumable)
        {
            if (GameManager.Instance.items.ContainsKey(item))
            {
                return;
            } 
        }
        if (item.price <= GameManager.Instance.coin)
        {
            GameManager.Instance.RemoveCoins(item.price);
            GameManager.Instance.AddOneItem(item);
        }
    }

    public void CloseShop()
    {
        GameManager.Instance.CloseShop();
    }
    
    

}
