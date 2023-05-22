
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


/// <summary>
/// This class is the main class of the CombatSystem
/// </summary>
public class BattleSystem : MonoBehaviour
{
    
    [SerializeField] private BattleState _state;
    [SerializeField] private BattleUI ui;
    [SerializeField] private Fighter player;
    [SerializeField] private Fighter enemy;
    [SerializeField] private AttackObject[] playerAttack;
    [SerializeField] private AttackObject[] enemyAttack;
    [SerializeField] private AttackMenu attackMenu;
    [SerializeField] private HealMenu healMenu;
    [SerializeField] private PerformAttack performAttack;
    [SerializeField] public AudioClip soundAttackSucceeded;
    [SerializeField] public AudioClip soundAttackFailed;

    private UnityAction<bool, int, int> onCombatEnd;
    public int[] _uses = { 0, 0, 0, 0 };
    
    public Fighter Player => player;
    public Fighter Enemy => enemy;
    public BattleUI Interface => ui;
    
    
    /// <summary>
    /// Represent the "life" of the player but it's a shared bar between player and enemy. 
    /// </summary>
    [SerializeField] private float appreciationPercentage;
    public float Appreciation
    {
        get
        {
            return appreciationPercentage * 100;
        }
        set
        {
            appreciationPercentage = value / 100;
        }
    }
    /// <summary>
    /// Use to decrease the life of the player
    /// eg : when the enemy attack.
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns></returns>
    public bool DecreasePlayerAppreciationBy(int percentage)
    {
        Appreciation -= (Appreciation * percentage) / 100;
        Interface.UpdateAppreciation(appreciationPercentage);
        return Appreciation > 0;
    }
    /// <summary>
    /// Use to decrease the enemy life.
    /// eg : when the player attack
    /// </summary>
    /// <param name="percentage"></param>
    /// <returns></returns>
    public bool DecreaseEnemyAppreciationBy(int percentage)
    {
        Appreciation += (Appreciation * percentage) / 100;
        Interface.UpdateAppreciation(appreciationPercentage);
        return Appreciation < 100;
    }
    
    
    //variable for cooldown
    private bool buttonCooldown;
    public void ResetCooldown()
    {
        buttonCooldown = false;
    }



    /// <summary>
    /// This function start a battle against an enemy.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    public void StartABattle(Fighter player, Fighter enemy, UnityAction<bool, int, int> onEnd)
    {
        this.player = player;
        this.enemy = enemy;
        this.onCombatEnd = onEnd;
        Interface.Initialize(player, enemy);
        Appreciation = 50;
        _state = BattleState.Beginning;
        SoundManager.Instance.StopMusic();
        StartCoroutine(BeginBattle());
    }

    public void OnAttackButton()
    {
        //need to implement a cooldown
        if (buttonCooldown) return;
        buttonCooldown = true;
        Invoke("ResetCooldown", 2f);
        if (_state != BattleState.PlayerTurn) return;
        Debug.Log("OnAttackButton");
        PlayerAttack();
    }

    /// <summary>
    /// For the moment this function is not connected to the inventory but must be.
    /// </summary>
    public void OnHealButton()
    {
        if (buttonCooldown) return;
        buttonCooldown = true;
        Invoke("ResetCooldown", 2.5f);
        if (_state != BattleState.PlayerTurn) return;

        void OnSelectHeal(Consumable consumable)
        {
            StartCoroutine(PlayerHeal(consumable));
        }
        
        healMenu.OpenMenu(OnSelectHeal);
    }

    /// <summary>
    /// This function begin the battle with dialogue.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeginBattle()
    {
        Debug.Log(enemy.unitName);
        Interface.SetDialogText($"You face {enemy.unitName} in a musical battle, may the best one win!");
        
        yield return new WaitForSeconds(2f);

        _state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    /// <summary>
    /// This function just shows a dialogue to choose an action.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerTurn()
    {
        Interface.SetDialogText("Choose an action.");
        yield break;
    }

    /// <summary>
    /// This function open the PlayerAttack menu and wait the player attack to attack
    /// </summary>
    /// <returns></returns>
    private void PlayerAttack()
    {
        bool success = false;
        UnityEvent<bool, AttackObject> onCompleteAttack = new UnityEvent<bool, AttackObject>();
        onCompleteAttack.AddListener(((arg0, arg1) => StartCoroutine(OnCompleteAttack(arg0,arg1))));
        attackMenu.OpenMenu(player,OnSelectAttack,this);

        void OnSelectAttack(AttackObject a)
        {
            if (a == null)
            {
                return;
            }
            Debug.Log("Selected attack");
            Debug.Log(a.input.sequence);
            player.RemoveInspiration(GetRemovedInspiration(a.rarity));
            StartCoroutine(performAttack.StartAttack(a.input.sequence.ToList(), a, onCompleteAttack));
        }
        
        IEnumerator OnCompleteAttack(bool succeeded,AttackObject attack)
        {
            Debug.Log("123");
            success = succeeded;
            bool isEnemyAlive = true;
            if (success)
            {
                Interface.SetDialogText("Attack succeeded !");
                isEnemyAlive = DecreaseEnemyAppreciationBy(GetDamage(attack.rarity));
                Debug.Log("marche ta mere1");
            }
            else
            {
                Interface.SetDialogText("Attack failed !");
                Debug.Log("marche ta mere2");
            }
            Debug.Log("marche ta mere3");
            yield return new WaitForSeconds(2);
            if (!isEnemyAlive)
            {
                _state = BattleState.Won;
                yield return StartCoroutine(EndGame());
            }
            else
            {
                yield return StartCoroutine(EnemyTurn());
            }
            
            
            
        }
    }

    private int GetDamage(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return 30;
            case Rarity.Hyped:
                return 60;
            case Rarity.Legendary:
                return 90;
            default:
                return 30;
        }
    }
    
    private int GetRemovedInspiration(Rarity rarity)
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
                return 4;
        }
    }


    private IEnumerator PlayerHeal(Consumable consumable)
    {
        if (consumable.addedResistance != 0)
            Interface.SetDialogText($"{Player.unitName} used {consumable.name} and gain {consumable.addedResistance} resistance.");
        else if (consumable.addedResistance != 0 && consumable.addedInspiration!= 0 )
            Interface.SetDialogText($"{Player.unitName} used {consumable.name} and gain {consumable.addedResistance} resistance and also gain {consumable.addedInspiration} inspiration.");
        else
            Interface.SetDialogText($"{Player.unitName} used {consumable.name} and gain {consumable.addedInspiration} inspiration.");
        
        player.AddInspiration(consumable.addedInspiration);
        player.AddResistance(consumable.addedResistance);
    
        yield return new WaitForSeconds(1f);

        _state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);
        Interface.SetDialogText($"{Enemy.unitName} attacks!");
        
        var isAlive = DecreasePlayerAppreciationBy(4);

        yield return new WaitForSeconds(1f);

        if (!isAlive)
        {
            _state = BattleState.Lost;
            StartCoroutine(EndGame());
        }
        else
        {
            _state = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }
    }

    private IEnumerator EndGame()
    {
        switch (_state)
        {
            case BattleState.Won:
                Interface.SetDialogText("You won the battle!");
                yield return new WaitForSeconds(5);
                SoundManager.Instance.PlayMusic();
                Destroy(gameObject);
                onCombatEnd(true, player.inspiration, player.resistance);
                break;
            case BattleState.Lost:
                Interface.SetDialogText("You were defeated.");
                yield return new WaitForSeconds(5);
                SoundManager.Instance.PlayMusic();
                Destroy(gameObject);
                onCombatEnd(false, player.inspiration, player.resistance);
                
                break;
            default:
                Interface.SetDialogText("The match was a stalemate!");
                break;
        }
    }
    
    
}
