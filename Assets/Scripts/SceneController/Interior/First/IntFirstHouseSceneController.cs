using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


//This class is responsible for handling every single event in the studio
public class IntFirstHouseSceneController : SceneController
{
    public GameObject quest1Prefab;
    public Transform quest1Point;
    private GameObject questInstanciated;

    public override void OnStart()
    {
        
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

    public override void Init()
    {
        
    }
}
