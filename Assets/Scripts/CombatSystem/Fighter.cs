using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter
{
    public string unitName;
    public float resistance;
    public float inspiration;
    public AttackObject[] Attacks; // all ingame usable attacks

    /// <summary>
    /// Check inspiration, if < 0 then return false.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
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

    public void AddInspiration(float x)
    {
        inspiration += x;
        if (inspiration > 10)
        {
            inspiration = 10;
        }
    }
    
    public void RemoveResistance(float x)
    {
        resistance -= x;
        if (resistance < 0)
        {
            resistance = 0;
        }
    }

    public void AddResistance(float x)
    {
        resistance += x;
        if (resistance > 10)
        {
            inspiration = 10;
        }
    }

    public Fighter(string unitName, float resistance, float inspiration, AttackObject[] attack)
    {
        this.unitName = unitName;
        this.inspiration = inspiration;
        this.resistance = resistance;
        this.Attacks = attack;
    }
}
