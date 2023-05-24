
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
    public TMP_Text healInspi;
    public TMP_Text healResist;
    public TMP_Text healDescription;
    
    public void Change(Item item)
    {
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
        }
        if (item is Consumable)
        {
            if (healInspi is not null) healInspi.text = ((Consumable)item).addedInspiration.ToString();
            if (healResist is not null) healResist.text = ((Consumable)item).addedResistance.ToString();
            if (healDescription is not null) healDescription.text = ((Consumable)item).description;
            if (refImage is not null) refImage.sprite = item.image;
        }
    }

    public void Change(Quest quest)
    {
        name.text = quest.information.name;
        description.text = quest.information.description;
    }
}
