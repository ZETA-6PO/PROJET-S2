using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// The singleton class.
/// </summary>
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    public GameData gameData;
      private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake() 
    {
        if (Instance != null) 
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
              //LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        //SaveGame();
    }
    
    /// <summary>
    /// Load an existing game or create a new game
    /// </summary>
    /// <returns></returns>
    
    public bool LoadGame()
    {
        // load any saved data from a file using the data handler
        gameData = dataHandler.Load();
        
        //Debug.Log($"1 coin : {gameData.coin}");
        
        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (gameData == null && initializeDataIfNull) 
        {
            gameData = new GameData();
            GameManager.Instance.LoadData(gameData);
            return false;
        }

        GameManager.Instance.LoadData(gameData);
        //Debug.Log($"2 coin : {gameData.coin}");
        return true;
    }

    public void SaveGame()
    {
        // if we don't have any data to save, log a warning here
        if (this.gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }
        
        GameManager.Instance.SaveData(gameData);
        
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }
    
    public bool HasGameData() 
    {
        return gameData != null;
    }
}
