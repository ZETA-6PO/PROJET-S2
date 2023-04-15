using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Item/AttackObject", order = 1)]
public class AttackObject: Item
{
    /// <summary>
    /// This class is inteded to 
    /// </summary> // an int from 0 to MaxUse representing the use time of the attack
    private int MaxUse;
    public Rarity Rarity; // image in combat

    public AudioClip sound;
    
    public string DialogWhenAttackFail; //le text mis quand un player rate lattack
    public string DialogWhenAttackSucceed; //le text mis quand un player reussi lattack

    public struct AttackInput
    {
        public KeyCode[] sequence;
        public int time;
    }

    public AttackInput input;
    public AttackObject(
        KeyCode[] touchSequences,
        int seconds
        )
    {
        input.sequence = touchSequences;
        input.time = seconds;
    }
}

public enum Rarity
{
    Common, //unrarest
    Hyped,
    Legendary//rarest
}
