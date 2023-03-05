using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ExtFirstSceneController : SceneController
{

    public override void Init()
    {
        entryPoint.position = GameManager.Instance.LastPositionOnMap;
    }
}
