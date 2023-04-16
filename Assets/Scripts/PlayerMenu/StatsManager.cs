using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    // player name
    public TMP_Text name; 
    
    //playerImages changer
    public List<Sprite> images;
    private int _current;
    public Image img;
    
    //player stats gaugebar
    public GaugeBar resistanceBar;
    public GaugeBar inspirationBar;
    public GaugeBar fameBar;

    private bool isSelectorOpen = false;
    
    //attacks images
    public Image[] slots = new Image[4];
    //attacks selector
    public AttackSelector prefabSelector;
    private AttackSelector refSelector;
    public Item[] equiped;
    public Dictionary<Item, int> inventory;

    public void UpdateStat(Item[] equiped,Dictionary<Item,int> items)
    {
        inventory = items;
        name.text = GameManager.Instance.playerName;
        _current = 0;
        if (images.Count > 0)
        {
            img.sprite = images[_current];
            img.gameObject.SetActive(true);
        }
        else
        {
            img.gameObject.SetActive(false);
        }

        var x = GameManager.Instance.playerResistance;
        resistanceBar.BarUpdate(x,10);
        inspirationBar.BarUpdate(GameManager.Instance.playerInspiration,10);
        fameBar.BarUpdate(GameManager.Instance.playerFame,10);
        for (int i = 0; i < 4; i++)
        {
            if (equiped[i] is not null)
            {
                slots[i].sprite = equiped[i].image;
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
    public void TurnRight()
    {
        if (images.Count > 0)
        {
            _current += 1;
            if (_current >= images.Count) _current = 0;
            img.sprite = images[_current];
        }
    }
    
    public void TurnLeft()
    {
        if (images.Count > 0)
        {
            _current -= 1;
            if (_current < 0) _current = images.Count-1;
            img.sprite = images[_current];
        }
    }

    public void SlotClicked(int i)
    {
        if (isSelectorOpen)
            return;

        isSelectorOpen = true;
        
        
        OpenSelector(ToSelect(),i);
    }
    private void CloseSelector()
    {
        if (!isSelectorOpen)
            return;
        
        isSelectorOpen = false;
        
        
        Destroy(refSelector.gameObject);
        refSelector = null;
    }

    private List<AttackObject> ToSelect()
    {
        equiped = GameManager.Instance.stuff;
        List<AttackObject> list = new List<AttackObject>();
        foreach (Item item in inventory.Keys)
        {
            if (item is AttackObject && !equiped.Contains(item)) list.Add((AttackObject)item); 
        }
        return list;
    }

    private void OpenSelector(List<AttackObject> toSelect, int i)
    {
        
        if (refSelector is null)
        {
            refSelector = Instantiate(prefabSelector,gameObject.transform);
            AttackSelector selectorComponent = refSelector.GetComponent<AttackSelector>();
            selectorComponent.Initialise(toSelect,this,i);
        }
    }

    public void ChangeAttack(int index,AttackObject attack)
    {
        slots[index-1].gameObject.SetActive(true);
        slots[index - 1].sprite = attack.image;
        GameManager.Instance.ChangeStuff(index, attack);
        CloseSelector();
    }

    
}
