using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public string unitName;
    public float resistance;
    public float inspiration;
    public AttackObject[] Attacks; // all ingame usable attacks

    //check inspiration point, if < 0 return false
    public bool RemoveInspiration(float x)
    {
        inspiration -= x;
        if (inspiration < 0)
        {
            inspiration = 0;
            return false;
        }

        return true;
    }
}
