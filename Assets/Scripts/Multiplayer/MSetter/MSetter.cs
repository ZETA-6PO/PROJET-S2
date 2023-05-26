using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSetter : MonoBehaviour
{
    
    public MultiplayerFighter[] fighters;

    [SerializeField] private GameObject spacePrefab;
    private MDescriptionSpace _space;
    [SerializeField] private GameObject panelPrefab;
    private MCharacterPanel _panel;
    [SerializeField] private GameObject printerPrefab;
    private MListPrinter _printer;
    [SerializeField] private GameObject chooser;
    [SerializeField] private GameObject slotPrefab;

    private MultiplayerFighter _fighter;

    public void Start()
    {
        foreach (MultiplayerFighter fighter in fighters)
        {
            AddSlot(fighter);
        }
        OpenPanel();
        OpenPrinter();
        OpenSpace();
        _fighter = fighters[0];
        ShowFighter();
    }

    private void OpenPrinter()
    {
        GameObject obj = Instantiate(printerPrefab,transform);
        _printer = obj.GetComponent<MListPrinter>();
        _printer.Initialize(this);
    }

    private void OpenSpace()
    {
        GameObject obj = Instantiate(spacePrefab,transform);
        _space = obj.GetComponent<MDescriptionSpace>();
        _space.Initialize(this);
    }

    private void OpenPanel()
    {
        GameObject obj = Instantiate(panelPrefab,transform);
        _panel = obj.GetComponent<MCharacterPanel>();

    }
    private void AddSlot(MultiplayerFighter fighter)
    {
        GameObject slot = Instantiate(slotPrefab, chooser.transform);
        MSlot newSlot = slot.GetComponent<MSlot>();
        newSlot.Initialize(fighter,this);
    }
    private void ShowFighter()
    {
        _panel.Change(_fighter);
        _printer.UpdateList(_fighter.Inventory);
    }

    public void ChangeSpace(Item item)
    {
        _space.Change(item);
    }
    public void ChangeCurrentFighter(MultiplayerFighter fighter)
    {
        _fighter = fighter;
        ShowFighter();
    }
    
    public MultiplayerFighter TakeFighter()
    {
        return _fighter;
    }
    
}
