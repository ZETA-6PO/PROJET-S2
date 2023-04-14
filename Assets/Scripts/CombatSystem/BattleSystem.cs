
using System.Collections;
using System.IO;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    private BattleState _state;
    [SerializeField]
    private BattleUI ui;
    [SerializeField]
    private Fighter player;
    [SerializeField]
    private Fighter enemy;

    [SerializeField]
    private AttackMenu attackMenu;
    
    public Fighter Player => player;
    public Fighter Enemy => enemy;
    public BattleUI Interface => ui;


    private bool buttonCooldown;

    public void ResetCooldown()
    {
        buttonCooldown = false;
    }
    //a float btw 0-1 representing the appreciation of the public
    [SerializeField]
    private float appreciationPercentage;

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

    public bool DecreasePlayerAppreciationBy(int percentage)
    {
        Appreciation -= (Appreciation * percentage) / 100;
        Interface.UpdateAppreciation(appreciationPercentage);
        return Appreciation > 0;
    }
    
    public bool DecreaseEnemyAppreciationBy(int percentage)
    {
        Appreciation += (Appreciation * percentage) / 100;
        Interface.UpdateAppreciation(appreciationPercentage);
        return Appreciation < 100;
    }
    
    
    

    private void Start()
    {
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
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (buttonCooldown) return;
        buttonCooldown = true;
        Invoke("ResetCooldown", 2.5f);
        if (_state != BattleState.PlayerTurn) return;
        StartCoroutine(PlayerHeal());
    }

    private IEnumerator BeginBattle()
    {
        Interface.SetDialogText($"You face {enemy.unitName} in a musical battle, may the best one win!");
        
        yield return new WaitForSeconds(2f);

        _state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        Interface.SetDialogText("Choose an action.");
        yield break;
    }

    private IEnumerator PlayerAttack()
    {
        bool good = false;
        attackMenu.OpenMenu(Player,(AttackObject a) =>
        {
            Debug.Log("caca");
            good = true;
        });
        yield return new WaitUntil(() => good);
        StartCoroutine(EnemyTurn());
        /*bool isAlive = DecreaseEnemyAppreciationBy(4);
        yield return new WaitForSeconds(1f);
        if (!isAlive)
        {
            _state = BattleState.Won;
            StartCoroutine(EndGame());
        }
        else
        {
            _state = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }*/
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
}
