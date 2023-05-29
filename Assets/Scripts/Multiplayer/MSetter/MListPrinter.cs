using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MListPrinter : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private GameObject _content;
    private MSetter _launcher;
    private List<GameObject> _cells;
    

    public void Initialize(MSetter launcher)
    {
        _launcher = launcher;
    }

    public void UpdateList(Dictionary<Item,int> items)
    {
        //Debug.Log("dico lenght :" + items.Count);
        foreach (Item item in items.Keys)
        {
            AddCell(item,items[item]);
        }
    }

    private void AddCell(Item item, int n)
    {
        GameObject cell = Instantiate(_cellPrefab, _content.transform);
        MItemCell itemCell = cell.GetComponent<MItemCell>();
        itemCell.Initialize(item,n,this);
    }

    public void ShowItem(Item item)
    {
        _launcher.ChangeSpace(item);
    }

    private void Clean()
    {
        while (_cells.Count>0)
        {
            
        }
    }
}
