using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is responsible for handling every single event in the studio
public class InteriorMapsStudioPoorSceneController : MonoBehaviour
{


    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public Transform entryPoint;
    
    private GameObject playerInstanciated;
    private GameObject cameraInstanciated;
    
    void Start()
    {
        
        //instanciate the scene
        cameraPrefab.SetActive(false);
        playerInstanciated = Instantiate(playerPrefab, entryPoint.position, Quaternion.identity);
        cameraInstanciated = Instantiate(cameraPrefab, entryPoint.position, Quaternion.identity);
        CameraFollow cf = cameraInstanciated.GetComponent<CameraFollow>();
        cf.player = playerInstanciated;
        cameraInstanciated.SetActive(true);
    }


}
