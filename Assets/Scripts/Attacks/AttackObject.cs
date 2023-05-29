using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine; 

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Item/AttackObject", order = 1)]
public class AttackObject: Item
{
    /// <summary>
    /// This class is inteded to 
    /// </summary> // an int from 0 to MaxUse representing the use time of the attack

    public Rarity rarity; // image in combat
    public Effect effect;
    public int InspirationCost
    {
        get
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return 1;
                case Rarity.Hyped:
                    return 2;
                case Rarity.Legendary:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public int MaxUse
    {
        get
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return 10;
                case Rarity.Hyped:
                    return 5;
                case Rarity.Legendary:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public int ResistanceImpact
    {
        get
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return 1;
                case Rarity.Hyped:
                    return 2;
                case Rarity.Legendary:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public AudioClip sound;

    public int damage;//the amount of damage made
    

    [Serializable]
    public struct AttackInput
    {
        public int time;
    }

    public AttackInput input;
    public AttackObject(
        KeyCode[] touchSequences,
        int seconds
        )
    {
        input.time = seconds;
    }
    
    
    
    
}

public enum Rarity
{
    Common, //unrarest
    Hyped,
    Legendary//rarest
}


// effets de status
public enum Effect
{
    FreakOut, //effet de panic qui fait que le personnage ne pourra pas jouer pendant (RANDOM : 1 - 3 tours)
    Mistake, //effet de fausse note qui fait que le joueur va jouer faux et fait qu'il prends un malus d'appreciation a chq++++ (RANDOM : 1 - 3 tours)
    Stressed, //effet de stress qui fait que vous le joueur doit jouer toutes ces attaques en moitier moins de temps (RANDOM : 1 - 3 tours)
    None
}
