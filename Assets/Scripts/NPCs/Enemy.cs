using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Fighter fighter; //sprite waiting
    public string triggerName;
    public string aka;//description or citation 
    public int money;
    public EnemyScreen screenPrefab;
    private EnemyScreen refScreen;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (refScreen is null)
        {
            refScreen = Instantiate(screenPrefab,gameObject.transform);
            refScreen.Initialise(this);
        }
    }

    public void LaunchBattle()
    {
        Debug.Log("LauchBattle");
        GameManager.Instance.StartACombat(fighter, win =>
        {
            Destroy(refScreen.gameObject);
            refScreen = null;
            if (win)
            {
                GameManager.Instance.AddCoins(money);
                gameObject.SetActive(false);
            }
        });
    }
}
