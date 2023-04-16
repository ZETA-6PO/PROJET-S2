using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//This class is responsible for handling every single event in the shop
public class IntFirstMusicStoreSceneController : SceneController
{
//event function triggered when touching exiting point 
    public void OnExitScene()
    {
        SceneManager.LoadScene("ExtFirstScene");
    }

    public override void Init()
    {
    }
}
