using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntBakeryPoor : SceneController
{
//event function triggered when touching exiting point 
    public void OnExitScene(int i)
    {
        SceneManager.LoadScene("ExtFirstScene");
    }


    public override void Init()
    {
    }
}
