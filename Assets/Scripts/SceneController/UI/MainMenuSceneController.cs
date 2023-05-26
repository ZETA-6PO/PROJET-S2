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
		if (GameManager.Instance == null)
			Instantiate(questManagerPrefab, Vector3.zero, Quaternion.identity);
	}
	
	
	public void StartNewGame()
	{
		if (DataPersistenceManager.Instance.LoadGame())
		{
			SceneManager.LoadScene(DataPersistenceManager.Instance.gameData.lastMap);
		}
		else
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
