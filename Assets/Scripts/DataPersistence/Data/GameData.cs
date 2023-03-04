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
    public Map lastMap;
    public Vector3 lastPlayerPosition;

    //PROGRESSIONS STATES
    public bool hasDoneTheIntro;

    /// <summary>
    /// This constructor is responsible for all the default value when
    /// a new game start.
    /// </summary>
    public GameData()
    {
        playerName = "Player";
        lastMap = Map.First;
        lastPlayerPosition = new Vector3(0, 0, 0);
        hasDoneTheIntro = false;
    }
    
}
