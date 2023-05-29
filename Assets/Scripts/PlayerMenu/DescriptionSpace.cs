﻿
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DescriptionSpace : MonoBehaviour
{
    public Image refImage;
    public TMP_Text name;
    public TMP_Text price;
    public TMP_Text description;
    public TMP_Text attackDamages;
    public TMP_Text attackCost;
    public TMP_Text attackEffect;
    public TMP_Text attackRarity;
    public TMP_Text attackResist;
    public TMP_Text attackMaxUse;
    public TMP_Text healInspi;
    public TMP_Text healResist;
    public GameObject useButton;
    private Item _item;
    
    public void Change(Item item)
    {
        _item = item;
        if (refImage is not null)refImage.sprite = item.image;
        if (name is not null)name.text = item.name;
        if (price is not null) price.text = item.price.ToString();
        if (description is not null) description.text = item.description;
        if (item is AttackObject)
        {
            if (attackDamages is not null) attackDamages.text = ((AttackObject)item).damage.ToString();
            if (attackCost is not null) attackCost.text = ((AttackObject)item).InspirationCost.ToString();
            if (attackEffect is not null) attackEffect.text = ((AttackObject)item).effect.ToString();
            if (attackRarity is not null) attackRarity.text = ((AttackObject)item).rarity.ToString();
            if (attackResist is not null) attackResist.text = ((AttackObject)item).ResistanceImpact.ToString();
            if (attackMaxUse is not null) attackMaxUse.text = ((AttackObject)item).MaxUse.ToString();
            if (useButton is not null) useButton.SetActive(false);
        }
        if (item is Consumable)
        {
            if (healInspi is not null) healInspi.text = ((Consumable)item).addedInspiration.ToString();
            if (healResist is not null) healResist.text = ((Consumable)item).addedResistance.ToString();
            if (refImage is not null) refImage.sprite = item.image;
            if (useButton is not null) useButton.SetActive(true);
        }
    }

    public void Change(Quest quest)
    {
        name.text = quest.information.name;
        description.text = quest.information.description;
    }
    
    public void Use()
    {
        if (_item is Consumable)
        {
            GameManager.Instance.UseItem((Consumable)_item);
            GameManager.Instance.CloseInventory();
        }
    }
}
