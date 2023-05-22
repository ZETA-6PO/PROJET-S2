using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScreen : MonoBehaviour
{
    public TMP_Text pseudo;
    public TMP_Text description;
    public Animator anim;
    private Enemy enemy;

    public void Initialise(Enemy e)
    {
        enemy = e;
        pseudo.text = enemy.fighter.unitName;
        description.text = enemy.aka;
        anim.SetTrigger(enemy.triggerName);
    }
    
    public void OnClickBattle()
    {
        Debug.Log("LauchBattle");
        enemy.LaunchBattle();
    }
}
