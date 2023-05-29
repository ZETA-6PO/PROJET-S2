using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is one of the most important class of the game.
/// It handles all the quest, menus, inventory system.
/// </summary>
public class GameManager : MonoBehaviour{
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

    private string lastMap; //this string represent the last ext map the player where on
    private string lastPositionOnLastMap; //this strings represent the last position of the player on the ext map


    public Item[] existingItem;
    
    
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


    public Item TempAddSingleItem;
    
    

    public Sprite playerSprite;


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

    //lTHE INVENTORY AS A DICTIONNARY
    public Dictionary<Item, int> items { get; set; } = new Dictionary<Item, int>();

    public Dictionary<Consumable, int> heal
    {
        get
        {
            Dictionary<Consumable, int> dico = new Dictionary<Consumable, int>();
            foreach (Item item in items.Keys)
            {
                if (item is Consumable)
                {
                    dico[(Consumable)item] = items[item];
                }
            }
            return dico;
        }
    }

    public AttackObject[] stuff  = new AttackObject[4];
    public Fighter opponent;

    public string playerName;
    public int playerResistance;
    public int playerInspiration;
    public int playerFame;
    public bool isWaypointActive = false;
    public Vector3 displayedWaypoint;
    public Time_Gestion refTime_Gestion;


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

    public void UseItem(Consumable c)
    {
        playerInspiration += c.addedInspiration;
        playerResistance += c.addedResistance;
        RemoveItem(c);
    }

    /// <summary>
    /// This function check wether or not the player has inspiration heal item in his inventory.
    /// </summary>
    public bool HasInspirationConsumable()
    {
        return items.Keys.Any((item =>
        {
            if (item is Consumable)
            {
                Consumable c = (Consumable)item;
                return c.addedInspiration > 0;
            }

            return false;
        }));
    }
    /// <summary>
    /// This function check wether or not the player has resistance heal item in his inventory.
    /// </summary>
    public bool HasResistanceConsumable()
    {
        return items.Keys.Any((item =>
        {
            if (item is Consumable)
            {
                Consumable c = (Consumable)item;
                return c.addedInspiration > 0;
            }

            return false;
        }));
    }

    public bool HasConsumable()
    {
        return HasInspirationConsumable() || HasResistanceConsumable();
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
        foreach (Item item in list) AddOneItem(item);
    }
    
    public void AddItems(List<Consumable> list)
    {
        foreach (Consumable item in list) AddOneItem(item);
    }
    
    public void AddCoins(int n)
    {
        coin += n;
        refATH.UpdateCoin();
    }
    public void RemoveCoins(int n)
    {
        coin -= n;
        refATH.UpdateCoin();
    }


    ////////////////////////////////////////////////////////
    /// All those variable handle the state of the quest.///
    ////////////////////////////////////////////////////////
    public List<Quest> quests;




    public void OnSceneLoad()
    {
        foreach (var quest in quests.Where(quest => quest.Active )) 
        {
            quest.OnLoadScene(SceneManager.GetActiveScene().name);
        }
        
        // Instatiate the ATH for the coins
        refATH = Instantiate(prefabATH); 
        refTime_Gestion.DisplayTime();
    }


    public void Update()
    {
        
        // THOSE ARE THE DEBUG KEY

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
            AddOneItem(TempAddSingleItem);
            
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            Instantiate(debugMenu);
        }

        if (Input.GetKeyUp(KeyCode.F4))
        {
            StartACombat(opponent, (arg0) =>
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
    
    public void OpenShop(List<Item> toBuy,string shopName)
    {
        if (isShopOpen)
            return;
        if (isPlayerMenuOpened)
            CloseInventory();
        isShopOpen = true;
        refShop = Instantiate(prefabShop, this.gameObject.transform.parent);
        refShop.UdpdateShop(toBuy,shopName);
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
        refATH.DisableATH();
        refCombat = Instantiate(prefabCombat);
        Fighter player = new Fighter(playerName, playerResistance, playerInspiration, stuff, playerSprite);
        refCombat.StartABattle(player, enemy, onCombatEnd);
        void onCombatEnd(bool win, int resistance, int inspiration)
        {
            playerResistance = resistance;
            playerInspiration = inspiration;
            refATH.EnableATH();
            onCombatFinished(win);
        }
    }
    
    ////////////////////////////////////////////////////////
    // ReSharper disable once InvalidXmlDocComment       ///
    /// Save and Load                                    ///
    ////////////////////////////////////////////////////////
    
    public void SaveData(GameData data)
    {
        data.coin = coin;
        data.fame = playerFame;
        data.inspiration = playerInspiration;
        data.resistance = playerResistance;

        
        int i = 0;
        foreach (var kv in items)
        {
            int e = 0;
            foreach (var kv2 in existingItem)
            {
                if (kv2 == kv.Key)
                {
                    data.inventory[e] = kv.Value;
                }

                e += 1;
            }

            i += 1;
        }

        i = 0;
        foreach (var obj in stuff)
        {
            if (obj == null)
            {
                data.stuff[i] = -1;
                continue;
            }
            int e = 0;
            foreach (var kv2 in existingItem)
            {
                if (kv2 == obj)
                {
                    data.stuff[i] = e;
                }
                e += 1;
            }
            i += 1;
        }

        PlayerController pc = FindObjectOfType<PlayerController>();

        if (pc is not null && SceneManager.GetActiveScene().name.StartsWith("Ext"))
        {
            data.lastPosition = pc.gameObject.transform.position;
            data.lastMap = SceneManager.GetActiveScene().name;
        }
        
        foreach (var dataQuest in data.quests)
        {
            dataQuest.active = quests.First(quest => quest.QuestId == dataQuest.questId).Active;
            dataQuest.completed = quests.First(quest => quest.QuestId == dataQuest.questId).Completed;
            dataQuest.questProperties = quests.First(quest => quest.QuestId == dataQuest.questId).SaveQuestProperties();
        }
    }
    
    public void LoadData(GameData data)
    {
        playerFame = data.fame;
        playerInspiration = data.inspiration;
        playerResistance = data.resistance;
        coin = data.coin;


        int i = 0;
        foreach (var kv in data.inventory)
        {
            
            if (kv > 0)
            {
                AddOneItem(existingItem[i]);
                items[existingItem[i]] += kv - 1;
            }
            i += 1;
        }

        i = 0;
        for (int j = 0; j < 4; j++)
        {
            if (data.stuff[i] != -1)
                stuff[i] = (AttackObject)existingItem[data.stuff[i]];
        }
        
        

        foreach (var dataQuest in data.quests)
        {
            
            var q = quests.First(quest => quest.QuestId == dataQuest.questId);
            Debug.Log($"q active : {q.Active} ");
            q.Active = dataQuest.active;
            Debug.Log($"q active : {q.Active} ");
            q.Completed = dataQuest.completed;
            q.LoadQuestProperties(dataQuest.questProperties);
        }
    }

}

