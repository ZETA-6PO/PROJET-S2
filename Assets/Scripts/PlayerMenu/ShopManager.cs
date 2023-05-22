using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TMP_Text shopName;
    public GameObject panel;
    public GameObject shopCellPrefab;
    public DescriptionSpace space;
    
    public void UdpdateShop(List<Item> toBuy)
    {
        shopName.text = "shop";
        space.Change(toBuy[0]);
        foreach (Item item in toBuy)
        {
            GameObject cellObject = Instantiate(shopCellPrefab, panel.transform);
            ShopCell cell = cellObject.GetComponent<ShopCell>();
            cell.InitialiseCell(item,this);
        }
    }
    
    public void UdpdateShop(List<Item> toBuy,string shop)
    {
        shopName.text = shop;
        space.Change(toBuy[0]);
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
            
            Debug.Log(GameManager.Instance.coin.ToString());
        }
    }

    public void CloseShop()
    {
        GameManager.Instance.CloseShop();
    }
    
    

}
