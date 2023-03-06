using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;


//This class is responsible for handling every single event in the studio
public class IntFirstHouseSceneController : SceneController
{
    public GameObject quest0Prefab;
    public Transform quest0Point;
    public GameObject quest1Prefab;
    public Transform quest1Point;
    private GameObject questInstanciated;
    

    public override void OnStart()
    {
        
        //********************
        //**STATE MANAGEMENT**
        //********************
        if (GameManager.Instance.HasDoneQ0 == false)
        {
            questInstanciated = Instantiate(quest0Prefab, quest0Point.position, Quaternion.identity);
            Q0 script = questInstanciated.GetComponent<Q0>();
            script.refDialogManager = dialogManager;
            StartCoroutine(script.demarre());
        }
        
        //Check if the player has done the intro.
        if (GameManager.Instance.HasDoneQ1 == false && GameManager.Instance.HasDoneQ0 == true && GameManager.Instance.TIsDoingQ1 == false)
        {
            //instanciate the quest prefab containing dad, mom and the collidebox + scripts
            questInstanciated = Instantiate(quest1Prefab, quest1Point.position, Quaternion.identity);
            Q1 script = questInstanciated.GetComponentInChildren<Q1>();
            script.refDialogManager = dialogManager;
        }

    }

    public override void Init()
    {
        if (GameManager.Instance.HasDoneQ0 == false)
        {
            entryPoint.position = quest0Point.position;
        }
    }

    //event function triggered when touching exiting point 
    public void OnExitScene()
    {
        if (GameManager.Instance.TIsDoingQ1)
            SceneManager.LoadScene("ExtFirstScene");
    }
}
