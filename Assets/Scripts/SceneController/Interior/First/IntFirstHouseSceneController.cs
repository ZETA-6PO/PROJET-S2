using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


//This class is responsible for handling every single event in the studio
public class IntFirstHouseSceneController : MonoBehaviour
{

    public PlayerController playerController;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public Transform entryPoint;
    public GameObject dialogPrefab;
    public GameObject quest1Prefab;
    public Transform quest1Point;
    
    private GameObject playerInstanciated;
    private GameObject cameraInstanciated;
    private GameObject questInstanciated;
    private GameObject dialogInstanciated;

    private DialogManager dialogManager;
    
    void Start()
    {
        
        //instanciate the scene
        cameraPrefab.SetActive(false);
        playerInstanciated = Instantiate(playerPrefab, entryPoint.position, Quaternion.identity);
        cameraInstanciated = Instantiate(cameraPrefab, entryPoint.position, Quaternion.identity);
        CameraFollow cf = cameraInstanciated.GetComponent<CameraFollow>();
        cf.player = playerInstanciated;
        playerController = playerInstanciated.GetComponent<PlayerController>();
        playerController.canMove = true;
        cameraInstanciated.SetActive(true);
        dialogInstanciated = Instantiate(dialogPrefab, Vector3.zero, Quaternion.identity);
        dialogManager = dialogInstanciated.GetComponent<DialogManager>();
        dialogManager.player = playerInstanciated;
        //********************
        //**STATE MANAGEMENT**
        //********************
        
        //Check if the player has done the intro.
        if (GameManager.Instance.HasDoneTheIntro == false)
        {
            //instanciate the quest prefab containing dad, mom and the collidebox + scripts
            questInstanciated = Instantiate(quest1Prefab, quest1Point.position, Quaternion.identity);
            HitBoxFirstDialog script = questInstanciated.GetComponentInChildren<HitBoxFirstDialog>();
            script.refDialogManager = dialogManager;
        }

    }


}
