using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntButcheryPoor : SceneController
{
    public void OnExitScene(int i)
    {
        SceneManager.LoadScene("ExtFirstScene");
    }


    public override void Init()
    {
    }
}