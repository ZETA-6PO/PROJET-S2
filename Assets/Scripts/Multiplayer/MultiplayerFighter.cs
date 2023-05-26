using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiplayerFighter
{
    //resistance
    public float resistance;
    //inspiration
    public float inspiration;
    //nickname
    public string nickName;
    //attack
    public AttackObject[] attacks;
    //attackUsage
    public int[] attacksUsage = new []{0,0,0,0};

    public MultiplayerFighter(string nickname, AttackObject[] attackObjects)
    {
        resistance = 10;
        inspiration = 10;
        nickName = nickname;
        attacks = attackObjects;
        foreach (var i in attacksUsage)
        {
            Debug.Log(i);
        }
    }
}
