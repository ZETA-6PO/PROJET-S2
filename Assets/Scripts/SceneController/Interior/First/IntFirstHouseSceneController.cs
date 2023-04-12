using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Sources;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;


//This class is responsible for handling every single event in the studio
public class IntFirstHouseSceneController : SceneController
{
    private parentsScript parentGO;
    public override void Init()
    {
        //entryPoint.position = DataPersistenceManager.Instance.gameData.lastPosition;
    }

    //event function triggered when touching exiting point 
    public void OnExitScene()
    {
        
    }
    
    
}
