using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Item item;
    public int count;
    public Image image;
    public TMP_Text name;
    public TMP_Text CountText;
    public InventoryManager Inventory;

    public void InitialiseCell(Item i,int number, InventoryManager inventory)
    {
        item = i;
        count = number;
        Inventory = inventory;
        image.sprite = item.image;
        name.text = item.name;
        CountText.text = count.ToString();
        
    }
    
    public void Clicked()
    {
        Inventory.space.Change(item);
    }

}
