using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttackObject", order = 1)]
public class AttackObject : ScriptableObject
{

    /// <summary>
    /// This class is inteded to 
    /// </summary>
    
    public string Name;
    public string Description;
    public int Use; // an int from 0 to MaxUse representing the use time of the attack
    public int MaxUse;
    public Rarity Rarity;
    
    public Sprite Icon; // icon in menu
    public Sprite ImageCombat; // image in combat

    //public TouchTimecode[] TouchTimecodeList;

    public string SoundPath;
    
    public string DialogWhenAttackFail; //le text mis quand un player rate lattack
    public string DialogWhenAttackSucceed; //le text mis quand un player reussi lattack

    public AttackObject(string name,
        string description,
        int use,
        int maxUse,
        Sprite iconPath,
        Sprite imageCombat,
        Rarity rarity,
        //Touch[] touchTimecodeList,
        string soundPath,
        string dialogWhenAttackFail,
        string dialogWhenAttackSucceed)
    {
        Name = name;
        Description = description;
        Use = use;
        MaxUse = maxUse;
        Icon = iconPath;
        ImageCombat = imageCombat;
        Rarity = rarity;
        //TouchTimecodeList = touchTimecodeList;
        SoundPath = soundPath;
        DialogWhenAttackFail = dialogWhenAttackFail;
        DialogWhenAttackSucceed = dialogWhenAttackSucceed;
    }

    
    
    
}

public enum Rarity
{
    Common, //unrarest
    Unpopular,
    Popular,
    Hyped //rarest
}
