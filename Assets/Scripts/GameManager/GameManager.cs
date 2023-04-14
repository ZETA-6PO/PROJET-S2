using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; private set; }
    

    /// <summary>
    /// All those variable handle the state of the quest.
    /// </summary>
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
        foreach (var quest in quests)
        {
            quest.OnLoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

