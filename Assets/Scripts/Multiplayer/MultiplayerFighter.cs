using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Fighter", menuName = "ScriptableObjects/Multiplayer Fighter", order = 4)]
public class MultiplayerFighter : ScriptableObject
{
    //resistance
    [NonSerialized]public float resistance;
    //max resistance
    public int maxResistance;
    //inspiration
    [NonSerialized]public float inspiration;
    //max Inspiration
    public int maxInspiration;
    //nickname
    public string nickName;
    //attacks
    public AttackObject[] attacks;
    //
    public List<Consumable> consumables;
    //attackUsage
    public int[] attacksUsage = new[] { 0, 0, 0, 0 };
    //image
    public Sprite image;
    //surname
    public string surname;

    public MultiplayerFighter(string nickname, AttackObject[] attackObjects)
    {
        resistance = 10;
        inspiration = 10;
        nickName = nickname;
        attacks = attackObjects;
    }
    public Dictionary<Item,int> Inventory 
    {
        get
        {
            Dictionary<Item, int> dico = new Dictionary<Item, int>();
            foreach (AttackObject attack in attacks)
            {
                dico[attack] = 1;
            }

            foreach (Consumable c in consumables)
            {
                if (!dico.ContainsKey(c))
                {
                    dico[c] = 1;
                }
                else
                {
                    dico[c] += 1;
                }
            }

            return dico;
        }
    }



    public void AddInspiration(int inspi)
    {
        inspiration += inspi;
        if (inspiration > maxInspiration) inspiration = maxInspiration;
    }

    public void AddResistance(int resistance)
    {
        this.resistance += resistance;
        if (this.resistance > maxResistance) this.resistance = maxInspiration;
    }

    /// <summary>
    ///  Remove inspiration point.
    /// </summary>
    /// <param name="inspi"></param>
    /// <returns>True if player has no more inspiration. False if there is inspiration remaining.</returns>
    public bool RemoveInspiration(int inspi)
    {
        this.inspiration -= inspiration;
        if (inspiration < 0)
        {
            this.inspiration = 0;
            return true;
        }

        return false;
    }
    
    /// <summary>
    ///  Remove resistance point.
    /// </summary>
    /// <param name="resistance"></param>
    /// <returns>True if player has no more resistance. False if there is resistance remaining.</returns>
    public bool RemoveResistance(int resistance)
    {
        this.resistance -= resistance;
        if (this.resistance < 0)
        {
            this.resistance = 0;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Use a consumable.
    /// </summary>
    /// <param name="cons"></param>
    public void UseConsumable(Consumable cons)
    {
        AddInspiration(cons.addedInspiration);
        AddResistance(cons.addedResistance);
    }
    
    
    
}
