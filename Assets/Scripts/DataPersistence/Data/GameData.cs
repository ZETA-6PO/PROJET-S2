using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This class is responsible for storing
///  every state or data that need to be saved;
/// </summary>
[System.Serializable]
public class GameData
{
    //GLOBAL VARIABLE
    public string playerName;
    
    public int playerLevel;

    //PROGRESSIONS VARIABLES
    public string lastMap;
    public Vector3 lastPosition;

    //PROGRESSIONS STATES
    public QuestData[] quests;

    public int coin;
    
    public int inspiration;

    public int resistance;
    
    public int fame;
    
    
    
    
    public int[] inventory;

    public int[] stuff;



    /// <summary>
    /// This constructor is responsible for all the default value when
    /// a new game start.
    /// </summary>
    public GameData()
    {
        playerName = "Player";
        lastMap = "ExtFirstScene";
        lastPosition = new Vector3(-69.5f, 17.5f, 0);
        quests = new[]
        {
            new QuestData("Q0", true, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q1", false, false, new [] { new QuestData.QuestProperty(){name = "test", value = "0"} }),
            new QuestData("Q2", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q3", false, false, new [] { new QuestData.QuestProperty(){name = "Win", value = "0"} }),
            new QuestData("Q4", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q5", false, false, new [] { new QuestData.QuestProperty(){name = "Win", value = "0"} }),
            new QuestData("Q6", false, false, new [] { new QuestData.QuestProperty(){name = "alreadySpeak", value = "0"} }),
            new QuestData("Q7", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q8", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q9", false, false, new [] { new QuestData.QuestProperty(){name = "had_pay", value = "0"} }),
            new QuestData("Q10", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q11", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q12", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q13", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q14", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q15", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q16", false, false, Array.Empty<QuestData.QuestProperty>()),
            new QuestData("Q17", false, false, Array.Empty<QuestData.QuestProperty>()),
        };
        coin = 0;
        inspiration = 10;
        resistance = 10;
        fame = 0;
        inventory = new int[26];
        stuff = new []{-1,-1,-1,-1};
    }
    
}
