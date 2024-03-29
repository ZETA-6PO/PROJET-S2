using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is responsible for letting the player go outside after entering a building.
/// </summary>
public class LoadExterior : MonoBehaviour
{
    public string sceneName;
    
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
