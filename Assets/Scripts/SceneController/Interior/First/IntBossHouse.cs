using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntBossHouse : SceneController
{
    public void OnExitScene(int i)
    {
        SceneManager.LoadScene("ExtLastScene");
    }
    
    public override void Init()
    {
    }
    
}
