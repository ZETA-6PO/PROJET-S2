using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttackObject", order = 1)]
public class AttackObject
{

    /// <summary>
    /// This class is inteded to 
    /// </summary>
    
    public string Name;
    public string Description;
    public int Use; // an int from 0 to MaxUse representing the use time of the attack
    public int MaxUse;
    public Rarity Rarity;
    public Sprite ImageCombat; // image in combat

    public string SoundPath;
    
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
    Unpopular,
    Popular,
    Hyped //rarest
}
