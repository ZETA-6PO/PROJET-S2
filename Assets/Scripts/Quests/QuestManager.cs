using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager Instance { get; private set; }

    [SerializeField]
    public List<Quest> quests; //this is the list of quest

    public List<string> activeQuest;

    public List<string> completedQuest;

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

    public void LoadData(GameData data)
    {
        completedQuest = data.completedQuestId;
        activeQuest = data.activeQuestId;
    }

    public void SaveData(GameData data)
    {
        data.completedQuestId = completedQuest;
        data.activeQuestId = activeQuest;
    }

    public void OnSceneLoad()
    {
        foreach (var questId in activeQuest)
        {
            quests.First(quest => quest.QuestId == questId).onLoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void OnCompleteQuest(string questId)
    {
        completedQuest.Add(questId);
        activeQuest.Remove(questId);
    }

    public void OnActivateQuest(string questId)
    {
        if(!activeQuest.Contains(questId))
            activeQuest.Add(questId);
        Debug.Log("COMMENT CA PAS MAL LES BZEZ");
    }
}

