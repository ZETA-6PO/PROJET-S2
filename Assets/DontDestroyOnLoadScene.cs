using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    public GameObject[] objects;
    private void Awake()
    {
        foreach (var item in objects)
        {
            DontDestroyOnLoad(item);
        }
    }
}
