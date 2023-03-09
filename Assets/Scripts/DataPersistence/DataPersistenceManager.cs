using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The singleton class.
/// </summary>
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    public GameData gameData;
    public TempData tempData;
    
    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

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

    public void NewGame()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.gameData = new GameData();
        this.tempData = new TempData();
    }

    public void LoadGame()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
    }
    //should be call whenever ugo la putain de sa mere a finit les menus
    public void SaveGame()
    {
        dataHandler.Save(gameData);
    }
}
