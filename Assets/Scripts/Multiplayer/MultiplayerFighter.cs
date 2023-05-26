using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Fighter", menuName = "ScriptableObjects/Multiplayer Fighter", order = 4)]
public class MultiplayerFighter : ScriptableObject
{
    //resistance
    public float resistance;
    //inspiration
    public float inspiration;
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
        foreach (var i in attacksUsage)
        {
            Debug.Log(i);
        }
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
}
