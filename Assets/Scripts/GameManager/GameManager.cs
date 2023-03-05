using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is responsible for saving all data throughout the scene
///
/// This should store every single state of the game such as
/// state variable.
/// </summary>
public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance { get; set; }
    [SerializeField] private Vector3 _lastPositionOnMap;


    //GLOBAL VARIABLE
    public string PlayerName { get; set; }
    public int Level { get; set; }
    
    
    //PROGRESSION VARIABLE
    public bool HasDoneQ0 { get; set; }
    
    public bool HasDoneQ1 { get; set; }
    public bool TIsDoingQ1 { get; set; }

    public Vector3 LastPositionOnMap
    {
        get => _lastPositionOnMap;
        set => _lastPositionOnMap = value;
    }

    public Map LastMap { get; set; }

    
    private void Awake()
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
        Debug.Log("Loading from GM");
        PlayerName = data.playerName;
        Level = data.playerLevel;
        HasDoneQ1 = data.HasDoneQ1;
        LastPositionOnMap = data.lastPlayerPosition;
        LastMap = data.lastMap;
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("Saving from GM");
        data.playerName = PlayerName;
        data.playerLevel = Level;
        data.HasDoneQ1 = HasDoneQ1;
        data.lastPlayerPosition = _lastPositionOnMap;
        data.lastMap = LastMap;
    }
}


public enum Map
{
    First,
    Second,
    Third
}
