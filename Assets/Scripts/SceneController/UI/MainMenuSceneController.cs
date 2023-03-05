using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneController : MonoBehaviour
{


	public GameObject gameManagerPrefab;
	public GameObject dataPersistenceManagerPrefab;
	public void Start()
	{
		if (GameManager.Instance == null)
			Instantiate(gameManagerPrefab, Vector3.zero, Quaternion.identity);
		if (DataPersistenceManager.instance == null)
			Instantiate(dataPersistenceManagerPrefab, Vector3.zero, Quaternion.identity);
	}
	
	
	public void StartNewGame()
	{
		Debug.Log("StartNewGame");
		//create a newgame files 
		DataPersistenceManager.instance.NewGame();
		
		
		//change scene to first scene where the intro will popup
		SceneManager.LoadScene("IntFirstHouseScene");
	}
	
	public void ChargeGame()
    {
	    Debug.Log("StartNewGame");
	    //create a newgame files 
	    DataPersistenceManager.instance.LoadGame();

	    if (!GameManager.Instance.HasDoneQ0)
	    {
		    SceneManager.LoadScene("IntFirstHouseScene");
	    }
	    
    }
	
    public void QuitGame()
    {
	    Debug.Log("QUIT!");
	    Application.Quit();
    }
    
}
