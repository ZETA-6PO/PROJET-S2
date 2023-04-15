using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    public Image image;
    public TMP_Text Name;
    public Item item;
    public ShopManager Manager;
    public GameObject buyButton;

    public void InitialiseCell(Item i,ShopManager manager)
    {
        Manager = manager;
        item = i;
        image.sprite = item.image;
        Name.text = item.name;
        //if((!item.consumable)&GameManager.Instance.items.ContainsKey(item)) buyButton.SetActive(false);
    }

    public void CellUpdate()
    {
        //if((!item.consumable)&GameManager.Instance.items.ContainsKey(item)) buyButton.SetActive(false);
    }
    public void CellClicked()
    {
        Manager.space.Change(item);
    }

    public void BuyClicked()
    {
        Manager.Buy(item);
        CellUpdate();
    }
}


