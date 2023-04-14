using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    public Image image;
    public TMP_Text Name;
    public Inventory inventory;
    public Item item;
    public ShopManager Manager;
    
    public void InitialiseCell(Item i,Inventory inv,ShopManager manager)
    {
        Manager = manager;
        item = i;
        inventory = inv;
        image.sprite = item.image;
        Name.text = item.name;
    }

    public void CellClicked()
    {
        Manager.space.Change(item);
    }

    public void BuyClicked()
    {
        Manager.Buy(item);
    }
}


