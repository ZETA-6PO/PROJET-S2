using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ExtFirstSceneController : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject dataPersistenceManager;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject dialogPrefab;
    private GameObject playerInstanciated;
    private GameObject cameraInstanciated;
    private GameObject dialogInstanciated;
    private Vector3 lastPlayerPosition;
    
    
    
    IEnumerator Start()
    {
        //TODO: this instance must be set defined from the menu not from here ITS A TEMPORARY SOLUTION
        if (GameManager.Instance == null)
            Instantiate(gameManagerPrefab, Vector3.zero, Quaternion.identity);
        if (DataPersistenceManager.instance == null)
            Instantiate(dataPersistenceManager, Vector3.zero, Quaternion.identity);

        yield return new WaitUntil(() => GameManager.Instance != null);
        
        //init variable
        lastPlayerPosition = GameManager.Instance.LastPositionOnMap;
        Debug.Log($"Making the player spawn at last pos; {lastPlayerPosition.x} {lastPlayerPosition.y} {lastPlayerPosition.z}");
        
        
        
        //instanciate the scene
        cameraPrefab.SetActive(false);
        playerInstanciated = Instantiate(playerPrefab, lastPlayerPosition, Quaternion.identity);
        cameraInstanciated = Instantiate(cameraPrefab, lastPlayerPosition, Quaternion.identity);
        CameraFollow cf = cameraInstanciated.GetComponent<CameraFollow>();
        cf.player = playerInstanciated;
        cameraInstanciated.SetActive(true);


        yield return new WaitForSeconds(2f);
    }
}
