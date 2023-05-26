using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MItemCell : MonoBehaviour
{
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _number;

    private Item _item;
    private MListPrinter _printer;

    public void Initialize(Item item, int number, MListPrinter printer)
    {
        _itemName.text = item.name;
        if (number < 2)
        {
            _number.text = "";
        }
        else
        {
            _number.text = number.ToString();
        }

        _printer = printer;
    }

    public void CellClicked()
    {
        _printer.ShowItem(_item);
    }
}
