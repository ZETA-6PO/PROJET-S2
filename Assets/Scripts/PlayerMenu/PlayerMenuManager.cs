using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField] private InventoryManager refInventoryManager;
    [SerializeField] private QuestManager refQuestManager;
    [SerializeField] private StatsManager refStatManager;

    
    
    public void UpdateAll(Dictionary<Item, int> items, List<Quest> quests,Item[] stuff)
    {
        refInventoryManager.UpdateInventory(items);
        refQuestManager.UpdateQuests(quests);
        refStatManager.UpdateStat(stuff,items);
    }
    
    
    public void Close()
    {
        GameManager.Instance.CloseInventory();
    }
    
    public void ButtonMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}