using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{
	public GameObject dataPersistenceManagerPrefab;
	public GameObject questManagerPrefab;
	public void Start()
	{
		if (DataPersistenceManager.Instance == null)
			Instantiate(dataPersistenceManagerPrefab, Vector3.zero, Quaternion.identity);
		if (QuestManager.Instance == null)
			Instantiate(questManagerPrefab, Vector3.zero, Quaternion.identity);
	}
	
	
	public void StartNewGame()
	{
		Debug.Log("StartNewGame");
		//create a newgame files 
		DataPersistenceManager.Instance.NewGame();
		
		//change scene to first scene where the intro will popup
		SceneManager.LoadScene("IntFirstHouseScene");
	}
	
	public void ChargeGame()
    {
	    Debug.Log("StartNewGame");
	    //create a newgame files 
	    DataPersistenceManager.Instance.LoadGame();
	    SceneManager.LoadScene(DataPersistenceManager.Instance.gameData.lastMap);

    }
	
    public void QuitGame()
    {
	    Debug.Log("QUIT!");
	    Application.Quit();
    }
    
}
