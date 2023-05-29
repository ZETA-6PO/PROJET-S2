using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityAction<MultiplayerFighter> onFighterSelected;



    private MultiplayerFighter _fighter;

    public void Start()
    {
        foreach (MultiplayerFighter fighter in fighters)
        {
            AddSlot(fighter);
        }
        OpenPanel();
        _fighter = fighters[0];
        ShowFighter();
    }

    private void OpenPrinter()
    {
        if (_printer is not null) Destroy(_printer.gameObject);
        GameObject obj = Instantiate(printerPrefab,transform);
        _printer = obj.GetComponent<MListPrinter>();
        _printer.Initialize(this);
    }

    private void OpenSpace()
    {
        if (_space is not null) Destroy(_space.gameObject);
        GameObject obj = Instantiate(spacePrefab,transform);
        _space = obj.GetComponent<MDescriptionSpace>();
    }

    private void OpenPanel()
    {
        GameObject obj = Instantiate(panelPrefab,transform);
        _panel = obj.GetComponent<MCharacterPanel>();
        _panel.Initialize(this);

    }
    private void AddSlot(MultiplayerFighter fighter)
    {
        GameObject slot = Instantiate(slotPrefab, chooser.transform);
        MSlot newSlot = slot.GetComponent<MSlot>();
        newSlot.Initialize(fighter,this);
    }
    private void ShowFighter()
    {
        OpenPrinter();
        OpenSpace();
        _panel.Change(_fighter);
        _printer.UpdateList(_fighter.Inventory);
        _space.Change(_fighter.attacks[0]);
    }

    public void ChangeSpace(Item item)
    {
        OpenSpace();
        _space.Change(item);
    }
    public void ChangeCurrentFighter(MultiplayerFighter fighter)
    {
        _fighter = fighter;
        ShowFighter();
    }
    
    public void TakeFighter()
    {
        onFighterSelected(_fighter);
    }
    
}
