using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Item item;
    public Image image;
    public TMP_Text name;
    public TMP_Text Count;
    public Inventory Inventory;

    public void InitialiseCell(Item i,Inventory inventory)
    {
        item = i;
        Inventory = inventory;
        image.sprite = item.image;
        name.text = item.name;
    }
    public void Update()
    {
        Count.text = Inventory.items[item].ToString();
    }
    
    public void Clicked()
    {
        Inventory.space.Change(item);
    }

}
