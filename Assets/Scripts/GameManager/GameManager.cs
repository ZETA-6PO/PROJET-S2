using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }

    public PlayerMenuManager prefabPlayerMenu;
    private PlayerMenuManager refPlayerMenu;

    public ShopManager prefabShop;
    private ShopManager refShop;

    public List<Item> invL = new List<Item>();
    private bool isPlayerMenuOpened = false;
    private bool isShopOpen = false;

    /// <summary>
    /// PlayerStats
    /// </summary>
    public int coin { get; private set; }

    public Dictionary<Item, int> items { get; set; } = new Dictionary<Item, int>();

    public Item[] stuff { get; set; } = new Item[4];

    public string playerName;
    public int playerResistance;
    public int playerInspiration;
    public int playerFame;
    public void AddOneItem(Item item)
    {
        if (items.Keys.Contains(item))
        {
            if (item.consumable)
            {
                items[item] += 1;
            }
           
        }
        else
        {
            items[item] = 1;
            
        }
    }

    /// <summary>
    /// Used to remove an item from the player's inventory.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (items[item] > 0)
        {
            if (item.consumable)
            {
                items[item] -= 1;
            }
        }
        if (!items.Keys.Contains(item))
        {
            Debug.Log("Not in inventory");
        }
    }

    /// <summary>
    /// This method is used to add Item to inventory.
    /// </summary>
    /// <param name="list"></param>
    public void AddItems(List<Item> list)
    {
        foreach (Item item in list)
        {
            AddOneItem(item);
        }

    }
    
    public void AddCoins(int n)
    {
        coin += n;
    }
    public void RemoveCoins(int n)
    {
        coin -= n;
    }


    ////////////////////////////////////////////////////////
    /// All those variable handle the state of the quest.///
    ////////////////////////////////////////////////////////
    public List<Quest> quests;
    public void LoadData(GameData data)
    {
        foreach (var dataQuest in data.quests)
        {
            var q = quests.First(quest => quest.QuestId == dataQuest.questId);
            q.Active = dataQuest.active;
            q.Completed = dataQuest.completed;
            q.LoadQuestProperties(dataQuest.questProperties);
        }
    }

    public void SaveData(GameData data)
    {
        foreach (var dataQuest in data.quests)
        {
            dataQuest.active = quests.First(quest => quest.QuestId == dataQuest.questId).Active;
            dataQuest.completed = quests.First(quest => quest.QuestId == dataQuest.questId).Completed;
            dataQuest.questProperties = quests.First(quest => quest.QuestId == dataQuest.questId).SaveQuestProperties();
        }
    }
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void OnSceneLoad()
    {
        foreach (var quest in quests.Where(quest => quest.Active )) 
        {
            quest.OnLoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log(quests[0].Active);
            if (isPlayerMenuOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (isShopOpen)
            {
                CloseShop();
            }
            else
            {
                //OpenShop(//laZikakaMixTape);
            }
        }
        
    }

    public void OpenInventory()
    {
        if (isShopOpen)
            return;
        if (isPlayerMenuOpened)
            return;
        isPlayerMenuOpened = true;
        refPlayerMenu = Instantiate(prefabPlayerMenu, this.gameObject.transform.parent);
        refPlayerMenu.UpdateAll(items, quests,stuff);
    }
    
    public void CloseInventory()
    {
        if (!isPlayerMenuOpened)
            return;
        isPlayerMenuOpened = false;
        Destroy(refPlayerMenu.gameObject);
        refPlayerMenu = null;
    }
    
    public void OpenShop(List<Item> toBuy)
    {
        if (isShopOpen)
            return;
        if (isPlayerMenuOpened)
            CloseInventory();
        isShopOpen = true;
        refShop = Instantiate(prefabShop, this.gameObject.transform.parent);
        refShop.UdpdateShop(toBuy);
    }
    
    public void CloseShop()
    {
        if (!isShopOpen)
            return;
        if (isPlayerMenuOpened)
            CloseInventory();
        isShopOpen = false;
        Destroy(refShop.gameObject);
        refShop = null;
    }

    public void ChangeStuff(int i, Item item)
    {
        if (stuff is not null && i > 0 & i < 5) stuff[i - 1] = item;
    }

    public void OnEnable()
    {
        AddItems(invL);
    }

    public bool IsInInventory(string name)
    {
        foreach (var it in items.Keys)
        {
            if (it.name == name)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItemName(string name)
    {
        foreach (var it in items.Keys)
        {
            if (it.name == name)
            {
                RemoveItem(it);
                return;
            }
        }
    }
}

