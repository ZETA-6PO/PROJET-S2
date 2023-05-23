using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter", menuName = "ScriptableObjects/Fighter", order = 3)]

public class Fighter : ScriptableObject
{
    public Sprite sprite;
    
    public string unitName;
    public int resistance;
    public int inspiration;
    public AttackObject[] Attacks; // all ingame usable attacks

    public Difficulty difficulty;
    
    

    /// <summary>
    /// Check inspiration, if < 0 then return false.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public bool RemoveInspiration(int x)
    {
        
        if (inspiration-x < 0)
        {
            return false;
        }
        else
        {
            inspiration -= x;
            return true;
        }

        
    }

    public void AddInspiration(int x)
    {
        inspiration += x;
        if (inspiration > 10)
        {
            inspiration = 10;
        }
    }
    
    public void RemoveResistance(int x)
    {
        resistance -= x;
        if (resistance < 0)
        {
            resistance = 0;
        }
    }

    public void AddResistance(int x)
    {
        resistance += x;
        if (resistance > 10)
        {
            inspiration = 10;
        }
    }

    public Fighter(string unitName, int resistance, int inspiration, AttackObject[] attack, Sprite sprite)
    {
        this.unitName = unitName;
        this.inspiration = inspiration;
        this.resistance = resistance;
        this.Attacks = attack;
    }
}


/// <summary>
/// level of difficulty of a fighter
/// </summary>
public enum Difficulty {
    Easy = 0,
    Normal = 1,
    Difficult = 2,
    Hard = 3,
}