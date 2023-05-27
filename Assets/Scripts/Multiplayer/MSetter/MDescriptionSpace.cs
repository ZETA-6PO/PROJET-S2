using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MDescriptionSpace : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _cellprefab;
    [SerializeField] private TMP_Text _itemName;
    
    
    public void Change(Item item)
    {
        _itemName.text = item.name;
        if (item is AttackObject)
        {
            AttackObject a = (AttackObject)item;
            AddCell("Damages :",a.damage.ToString());
            AddCell("Effect :",a.effect.ToString());
            AddCell("Rarity :",a.rarity.ToString());
            AddCell("Impact :",a.ResistanceImpact.ToString());
            AddCell("Max Use :",a.MaxUse.ToString());
            AddCell("Cost :",a.InspirationCost.ToString());
            
        }
        if (item is Consumable)
        {
            Consumable c = (Consumable)item;
            AddCell("Inspiration :",c.addedInspiration.ToString());
            AddCell("Resistance :",c.addedResistance.ToString());
        }
    }

    private void AddCell(string cellName,string var)
    {
        GameObject cell = Instantiate(_cellprefab,_content.transform);
        MStatCell statCell = cell.GetComponent<MStatCell>();
        statCell.Initialize(cellName,var);
    }
}
