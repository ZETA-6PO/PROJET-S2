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
    public Vector3 lastPosition;

    //PROGRESSIONS STATES
    public bool HasDoneQ0;
    public bool HasDoneQ1;

    /// <summary>
    /// This constructor is responsible for all the default value when
    /// a new game start.
    /// </summary>
    public GameData()
    {
        playerName = "Player";
        lastMap = Map.First;
        lastPosition = new Vector3(0, 0, 0);
        HasDoneQ0 = false;
        HasDoneQ1 = false;
    }
    
}


public enum Map
{
    First,
    Second,
    Third
}
