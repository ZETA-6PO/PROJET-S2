using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fighter", menuName = "ScriptableObjects/Fighter", order = 3)]

public class Fighter: ScriptableObject
{
    [SerializeField] public Sprite sprite;
    
    public string unitName;
    public int resistance;
    public int inspiration;
    public AttackObject[] Attacks; // all ingame usable attacks
    
    

    /// <summary>
    /// Check inspiration, if < 0 then return false.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public bool RemoveInspiration(int x)
    {
        inspiration -= x;
        if (inspiration < 0)
        {
            inspiration = 0;
            return false;
        }

        return true;
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

    public Fighter(string unitName, int resistance, int inspiration, AttackObject[] attack)
    {
        this.unitName = unitName;
        this.inspiration = inspiration;
        this.resistance = resistance;
        this.Attacks = attack;
    }
}
