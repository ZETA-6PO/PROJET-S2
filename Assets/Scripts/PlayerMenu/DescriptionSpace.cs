
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionSpace : MonoBehaviour
{
    public Image image;
    public TMP_Text name;
    public TMP_Text price;
    public TMP_Text description;
    
    public void Change(Item item)
    {
        image.sprite = item.image;
        name.text = item.name;
        if (!(price is null)) price.text = item.price.ToString();
        description.text = item.description;
    }

    public void Change(Quest quest)
    {
        name.text = quest.information.name;
        description.text = quest.information.description;
    }
}
