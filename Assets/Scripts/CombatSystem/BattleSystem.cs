
using System;
using System.Collections;
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
    [SerializeField] private AttackMenu attackMenu;
    [SerializeField] private PerformAttack performAttack;
    
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
            if (value < 0 || value > 100)
            {
                throw new InvalidDataException();
            }

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
    private void StartABattle(Fighter player, Fighter enemy)
    {
        this.player = player;
        this.enemy = enemy;
        Interface.Initialize(player, enemy);
        Appreciation = 50;
        _state = BattleState.Beginning;
        StartCoroutine(BeginBattle());
    }

    public void OnAttackButton()
    {
        //need to implement a cooldown
        if (buttonCooldown) return;
        buttonCooldown = true;
        Invoke("ResetCooldown", 2f);
        if (_state != BattleState.PlayerTurn) return;
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
        StartCoroutine(PlayerHeal());
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
        UnityEvent<bool> onCompleteAttack = new UnityEvent<bool>();
        onCompleteAttack.AddListener((arg0 => StartCoroutine(OnCompleteAttack(arg0))));
        attackMenu.OpenMenu(Player, OnSelectAttack);

        void OnSelectAttack(AttackObject a)
        {
            if (a == null)
            {
                return;
            }
            Debug.Log("Selected attack");
            StartCoroutine(performAttack.StartAttack(a.input.sequence.ToList(), 15, onCompleteAttack));
        }
        
        IEnumerator OnCompleteAttack(bool succeeded)
        {
            
            Debug.Log("123");
            success = succeeded;
            if (success)
            {
                Interface.SetDialogText("Attack succeeded !");
                DecreaseEnemyAppreciationBy(10);
                Debug.Log("marche ta mere1");
            }
            else
            {
                Interface.SetDialogText("Attack failed !");
                Debug.Log("marche ta mere2");
            }
            Debug.Log("marche ta mere3");
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(EnemyTurn());
            
            
        }
    }   

    private IEnumerator PlayerHeal()
    {
        Interface.SetDialogText($"{Player.unitName} feels renewed strength!");

        DecreaseEnemyAppreciationBy(4);
    
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
                break;
            case BattleState.Lost:
                Interface.SetDialogText("You were defeated.");
                break;
            default:
                Interface.SetDialogText("The match was a stalemate!");
                break;
        }
        yield break;
    }

    public void OnEnable()
    {
        StartCoroutine(tg());

        IEnumerator tg()
        {
            yield return new WaitForSeconds(2);
            StartABattle(new Fighter("A.NONYME", 10, 10, new []
            {
                new AttackObject(new []
                {
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.RightArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.RightArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow,
                    KeyCode.UpArrow,
                }, 10)
            }),new Fighter("I.GNOBLE", 10, 10, new []
            {
                new AttackObject(new []
                {
                    KeyCode.LeftArrow,
                    KeyCode.RightArrow
                }, 10)
            }));
        }
        
    }
}