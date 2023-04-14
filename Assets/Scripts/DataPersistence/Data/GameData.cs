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
    

    /// <summary>
    /// This constructor is responsible for all the default value when
    /// a new game start.
    /// </summary>
    public GameData()
    {
        playerName = "Player";
        lastMap = "IntFirstHouseScene";
        lastPosition = new Vector3(0, 0, 0);
        quests = new[]
        {
            new QuestData("Q0", true, false, Array.Empty<QuestData.QuestProperty>())
        };
    }
    
}
