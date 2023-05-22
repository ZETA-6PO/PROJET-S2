using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is one of the most important class of the game.
/// It handles all the quest, menus, inventory system.
/// </summary>
public class GameManager : MonoBehaviour, IDataPersistence
{
    ///////////////////////////////////////
    /// Singletons part do not edit pl. ///
    ///////////////////////////////////////
    public static GameManager Instance { get; private set; }
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
    
    
    ///////////////////////////////////////
    /// All the prefabs to instanciate. ///
    ///////////////////////////////////////
    public PlayerMenuManager prefabPlayerMenu;
    private PlayerMenuManager refPlayerMenu;

    public BattleSystem prefabCombat;
    private BattleSystem refCombat;
    
    public ShopManager prefabShop;
    private ShopManager refShop;

    public ATH prefabATH;
    private ATH refATH;


    public GameObject debugMenu;
    
    ///////////////////////////////
    /// Menu related variables. ///
    ///////////////////////////////
    private bool isPlayerMenuOpened = false;
    private bool isShopOpen = false;
    
    
    //////////////////////////////////
    /// Player stats and inventory ///
    //////////////////////////////////
    public int coin { get; private set; }

    public Dictionary<Item, int> items { get; set; } = new Dictionary<Item, int>();

    public AttackObject[] stuff  = new AttackObject[4];

    public string playerName;
    public int playerResistance;
    public int playerInspiration;
    public int playerFame;

    public bool isWaypointActive = false;
    public Vector3 displayedWaypoint;


    /// <summary>
    /// Used to add a single item to the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void AddOneItem(Item item)
    {
        if (item is AttackObject)
        {
            int slot = FindSlot();
            if (slot < 5)
            {
                stuff[slot] = (AttackObject)item;
            }
        }
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

    private int FindSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if (stuff[i] == null) return i;
        }
        return 5;
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
    
    
    public void OnSceneLoad()
    {
        foreach (var quest in quests.Where(quest => quest.Active )) 
        {
            quest.OnLoadScene(SceneManager.GetActiveScene().name);
        }
        
        // Instatiate the ATH for the coins
        refATH = Instantiate(prefabATH);
    }


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F3))
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
        
        if (Input.GetKeyUp(KeyCode.F2))
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

        if (Input.GetKeyUp(KeyCode.F1))
        {
            Instantiate(debugMenu);
        }

        if (Input.GetKeyUp(KeyCode.F4))
        {
            StartACombat(new Fighter("Opponent", 10, 10, new AttackObject[]{}), (arg0) =>
            {
                Debug.Log($"Combat end : win : {arg0}");
            });
        }
        
    }
    
    ////////////////////////////////////////////////////////
    /// All those methods handle the Inventory and Shop. ///
    ////////////////////////////////////////////////////////
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

    public void ChangeStuff(int i, AttackObject item)
    {
        if (stuff is not null && i > 0 & i < 5) stuff[i - 1] = item;
    }

    public void OnEnable()
    {
        //AddItems(invL);
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
    ////////////////////////////////////////////////////////
    // ReSharper disable once InvalidXmlDocComment       ///
    /// All those variable handle the CombatSystem       ///
    ////////////////////////////////////////////////////////

    public void StartACombat(Fighter enemy, UnityAction<bool> onCombatFinished)
    {
        refCombat = Instantiate(prefabCombat);
        Fighter player = new Fighter(playerName, playerResistance, playerInspiration, stuff);
        refCombat.StartABattle(player, enemy, onCombatEnd);
        
        void onCombatEnd(bool win, int resistance, int inspiration)
        {
            playerResistance = resistance;
            playerInspiration = inspiration;
            onCombatFinished(win);
        }
    }
    
    
    
}

