using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntStudioMedium : SceneController
{
    public void OnExitScene(int i)
    {
        SceneManager.LoadScene("ExtSecondScene");
    }
    
    public override void Init()
    {
    }
    
}
