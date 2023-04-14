using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("In UI")]
    public string itemName;
    public string itemDescription;
    public Sprite icon; // Icon in the menus
    [Header("In Shop")]
    public int price;
}
