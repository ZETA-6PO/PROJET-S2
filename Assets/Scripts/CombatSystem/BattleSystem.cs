
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


/// <summary>
/// This class is the main class of the CombatSystem
/// </summary>
public class BattleSystem : MonoBehaviour
{
    /// <summary>
    /// REFERENCES
    /// </summary>
    public BattleUI refUi;
    public AttackMenu refAttackMenu;
    public HealMenu refHealMenu;
    public PerformAttack refPerformAttack;
    public AudioClip combatMusic;
    public Fighter Player => player;
    public Fighter Enemy => enemy;
    public BattleUI Interface => refUi;
    
    public AudioClip soundAttackSucceeded;
    public AudioClip soundAttackFailed;
    private UnityAction<bool, int, int> onCombatEnd;



    /// <summary>
    /// STATS
    /// </summary>
    private EBattleState combatState; //it is the current state of the combat
    private float appreciation; //it is the current appreciation
    private Fighter player;
    private Fighter enemy;

    private bool isConsumableMessageActive = false;
    private bool hasDeclareForfeit = false;


    /// <summary>
    /// All the effect.
    /// </summary>
    private Dictionary<Effect, int> playerEffects = new Dictionary<Effect, int>()
    {
        {Effect.Mistake, 0},
        {Effect.Stressed, 0},
        {Effect.FreakOut , 0}
    };
    private Dictionary<Effect, int> enemyEffects = new Dictionary<Effect, int>(){
        {Effect.Mistake, 0},
        {Effect.Stressed, 0},
        {Effect.FreakOut , 0}
    };
    
    
    
    /// <summary>
    /// Add player appreciation.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Wether or not the player did win the match.</returns>
    private bool addPlayerAppreciation(float amount)
    {
        appreciation += amount;
        Interface.UpdateAppreciation(appreciation);
        return appreciation < 100;
    }
    
    /// <summary>
    /// Add enemy appreciation.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Wether or not the enemy did win the match.</returns>
    private bool addEnemyAppreciation(float amount)
    {
        appreciation -= amount;
        Interface.UpdateAppreciation(appreciation);
        return appreciation > 0;
    }
    
    
    
    
    /// <summary>
    /// This is the main function of the combat system, use it to start a combat.
    /// </summary>
    /// <param name="player">This is the player fighter.</param>
    /// <param name="enemy">This is the enemy fighter.</param>
    /// <param name="onEnd">This is the unityAction you can pass to do thing once the combat end, you</param>
    public void StartABattle(Fighter player, Fighter enemy, UnityAction<bool, int, int> onEnd)
    {
        this.player = player;
        this.enemy = enemy;
        //initialisation de l'interface
        Interface.Initialize(player, enemy);
        
        onCombatEnd = onEnd;
        appreciation = 50;
        Interface.UpdateAppreciation(appreciation);
        //set state pour demmarage
        combatState = EBattleState.Beginning;
        
        StartCoroutine(Combat());
        
        //stop la musique
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayCertainMusic(combatMusic);
    }


    /// <summary>
    /// This is the main loop of the combat.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Combat()
    {
        // this is the main loop 
        // it also handle the effect
        while (combatState != EBattleState.Lost || combatState != EBattleState.Won)
        {
            var _tmpCombatState = combatState; 
            yield return new WaitForSeconds(2f); //wait time between each turn
            if (playerEffects[Effect.Mistake] > 0)
            {
                refUi.SetDialogText($"{Player.unitName} will play wrong for {playerEffects[Effect.Mistake]-1} more rounds. As a result, the public appreciates him less, you lose appreciation.");
                bool _tempCheckKill = addEnemyAppreciation(10);
                if (!_tempCheckKill)
                {
                    yield return new WaitForSeconds(2f);
                    refUi.SetDialogText("Playing wrong made you lose the fight...");
                    combatState = EBattleState.Lost;
                }
            }
            
            if (enemyEffects[Effect.Mistake] > 0)
            {
                refUi.SetDialogText($"{enemy.unitName} will play wrong for {enemyEffects[Effect.Mistake]-1} more rounds. As a result, the public appreciates him less, he lose appreciation.");
                bool _tempCheckKill = addPlayerAppreciation(10);
                if (!_tempCheckKill)
                {
                    yield return new WaitForSeconds(2f);
                    refUi.SetDialogText($"Playing wrong made {enemy.unitName} loose the fight...");
                    combatState = EBattleState.Won;
                }
            }

            if (combatState == EBattleState.Beginning)
            {
                StartCoroutine(BeginBattle());
            }else if (combatState == EBattleState.PlayerTurn)
            {
                if (playerEffects[Effect.FreakOut] == 0)
                {
                    StartCoroutine(PlayerTurn());
                }
                else
                {
                    refUi.SetDialogText($"You have a stage fright for next {playerEffects[Effect.FreakOut]-1} rounds. He cannot attack or heal himself.");
                    yield return new WaitForSeconds(1);
                    combatState = EBattleState.EnemyTurn;
                }

                
            }else if (combatState == EBattleState.EnemyTurn)
            {
                if (enemyEffects[Effect.FreakOut] == 0)
                {
                    StartCoroutine(EnemyTurn());
                }
                else
                {
                    refUi.SetDialogText($"Enemy have a stage fright for next {enemyEffects[Effect.FreakOut]-1} rounds. He cannot attack or heal himself.");
                    yield return new WaitForSeconds(2);
                    combatState = EBattleState.PlayerTurn;
                }
            }
            
            //this two loop below ensure that we remove the effect on both player and enemy
            foreach (var keyValuePair in playerEffects)
            {
                if (playerEffects[keyValuePair.Key] != 0)
                    playerEffects[keyValuePair.Key] -= 1;
            }
            foreach (var keyValuePair in enemyEffects)
            {
                if (enemyEffects[keyValuePair.Key] != 0)
                    enemyEffects[keyValuePair.Key] -= 1;
            }
            yield return new WaitUntil((() => combatState != _tmpCombatState));
        }
        StartCoroutine(EndGame());
    }
    
    
    
    
    
    public void OnAttackButton()
    {
        if (refAttackMenu.gameObject.activeInHierarchy)
        {
            refAttackMenu.gameObject.SetActive(false);
        }
        else
        {
            if (combatState != EBattleState.PlayerTurn) return;
            PlayerAttack(); //open the attack menu
        }
       
    }
    public void OnHealButton()
    {
        if (refHealMenu.gameObject.activeInHierarchy)
            refHealMenu.gameObject.SetActive(false);
        else
        {
            if (combatState != EBattleState.PlayerTurn) return;

            if (!GameManager.Instance.HasConsumable())
            {
                IEnumerator NoConsumableMessage()
                {
                    refUi.SetDialogText("You don't have any consumable.");
                    yield return new WaitForSeconds(3f);
                    refUi.SetDialogText("Choose an action.");
                    isConsumableMessageActive = false;
                }
                if (!isConsumableMessageActive)
                {
                    isConsumableMessageActive = true;
                    StartCoroutine(NoConsumableMessage());
                }
                return;
                
            }
            
            
            //local function to call heal
            void OnSelectHeal(Consumable consumable)
            {
                StartCoroutine(PlayerHeal(consumable));
            }
            refHealMenu.OpenMenu(OnSelectHeal);
        }
        
    }

    public void OnClickForfeitButton()
    {
        if (hasDeclareForfeit || combatState != EBattleState.PlayerTurn)
        {
            return;
        }
        hasDeclareForfeit = true;
        IEnumerator LostMessage()
        {
            refUi.SetDialogText("Forfeiting has never been a winning strategy.");
            yield return new WaitForSeconds(3f);
            combatState = EBattleState.Lost;
            StartCoroutine(EndGame());
        }
        StartCoroutine(LostMessage());
    }
    
    
    
    

    /// <summary>
    /// This function begin the battle with dialogue.
    /// </summary>
    private IEnumerator BeginBattle()
    {
        
        Interface.SetDialogText($"You face {enemy.unitName} in a musical battle, may the best one win!");
        
        yield return new WaitForSeconds(2f);
        
        combatState = EBattleState.PlayerTurn;
        
        StartCoroutine(PlayerTurn());
    }

    /// <summary>
    /// This function just shows a dialogue to choose an action.
    /// </summary>
    private IEnumerator PlayerTurn()
    {
        refUi.EnableButtonForfeit();
        if (playerEffects[Effect.Stressed] > 0)
        {
            Interface.SetDialogText($"{player.unitName} is stressed for two more rounds.");
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Interface.SetDialogText("Choose an action.");
        }
        yield break;
    }

    /// <summary>
    /// This function open the PlayerAttack menu and wait the player attack to attack
    /// </summary>
    /// <returns></returns>
    private void PlayerAttack()
    {
        UnityEvent<bool, AttackObject> onCompleteAttack = new UnityEvent<bool, AttackObject>();
        onCompleteAttack.AddListener(((arg0, arg1) => StartCoroutine(OnCompleteAttack(arg0,arg1))));
        refAttackMenu.OpenMenu(player,OnSelectAttack,this);

        void OnSelectAttack(AttackObject a)
        {
            if (a == null) //if no attack is selected
            {
                return;
            }

            bool _hasNeededInspiration = player.RemoveInspiration(a.InspirationCost);
            refUi.UpdateInspiration(player.inspiration);

            if (!_hasNeededInspiration)
            {
                refUi.SetDialogText("You don't have the needed inspiration to launch this attack, heal or loose by forfeit.");
                return;
            }
            StartCoroutine(refPerformAttack.StartAttack( a, onCompleteAttack, playerEffects[Effect.Stressed] > 0));
        }
        
        IEnumerator OnCompleteAttack(bool succeeded,AttackObject attack)
        {
            bool isEnemyAlive = true;
            if (succeeded)
            {
                Interface.SetDialogText("Attack succeeded !");
                isEnemyAlive = addPlayerAppreciation(attack.damage);

            }
            else
            {
                Interface.SetDialogText($"{player.unitName} failed his attack !");
            }
            yield return new WaitForSeconds(2);
            if (!isEnemyAlive)
            {
                combatState = EBattleState.Won;
                yield return StartCoroutine(EndGame());
            }
            else
            {
                combatState = EBattleState.EnemyTurn;
            }
            
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
        refUi.UpdateInspiration(player.inspiration);
        player.AddResistance(consumable.addedResistance);
        refUi.UpdateResistance(player.resistance);
    
        yield return new WaitForSeconds(1f);

        combatState = EBattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// This function handle the enemy turn and is responsible for the IA part.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyTurn()
    {
        refUi.DisableButtonForfeit();
        // if enemy has no inspiration, boost enemy inspiration
        if (enemy.inspiration  < 4)
        {
            refUi.SetDialogText($"{player.unitName} uses an consumable to boost his inspiration.");
            enemy.AddInspiration(Random.Range(2,5));
            yield return new WaitForSeconds(2f);
            combatState = EBattleState.PlayerTurn;
            yield break;
        }

        // if enemy has the advantage and is low on resistance, he uses a consumable
        else if (appreciation < 30 && enemy.resistance < 5)
        {
            refUi.SetDialogText($"{player.unitName} uses an consumable to boost his resistance.");
            enemy.AddResistance(Random.Range(2,5));
            yield return new WaitForSeconds(2f);
            combatState = EBattleState.PlayerTurn;
            yield break;
        }
        
        // if the enemy is ultra low, he use his best attack
        else if (appreciation > 80 && enemy.difficulty == Difficulty.Difficult || enemy.difficulty == Difficulty.Hard)
        {
            AttackObject usedAttack = enemy.Attacks.OrderByDescending((o => o.rarity)).ToArray()[0];


            refUi.SetDialogText($"{enemy.unitName} uses {usedAttack.name}.");
            yield return new WaitForSeconds(2f);
            if (Random.Range(0f, 1f) < 0.8f) //80% CHANCES OF SUCCEEDING
            {
                refUi.SetDialogText($"{enemy.unitName} has successfully completed his attack.");
                addEnemyAppreciation(usedAttack.damage*(1-(1/20)*player.resistance));
                player.RemoveResistance(usedAttack.ResistanceImpact);
                refUi.UpdateResistance(player.resistance);
                yield return new WaitForSeconds(2f);
            }
            else
            {
                refUi.SetDialogText($"{enemy.unitName} has failed his attack.");
                yield return new WaitForSeconds(2f);
            }
        }
        else //else use the default attack
        {
            AttackObject usedAttack = enemy.Attacks.OrderBy((o => o.rarity)).ToArray()[Random.Range(0, enemy.Attacks.Length-1)];
            refUi.SetDialogText($"{enemy.unitName} uses {usedAttack.name}.");
            yield return new WaitForSeconds(2f);
            if (Random.Range(0f, 1f) <= 0.9f) //70% CHANCES OF SUCCEEDING
            {
                refUi.SetDialogText($"{enemy.unitName} has successfully completed his attack.");
                addEnemyAppreciation(usedAttack.damage*(1-(1/20)*player.resistance));
                player.RemoveResistance(usedAttack.ResistanceImpact);
                refUi.UpdateResistance(player.resistance);
                yield return new WaitForSeconds(1f);
            }
            else
            {
                refUi.SetDialogText($"{enemy.unitName} has failed his attack.");
                yield return new WaitForSeconds(1f);
            }
        }


        combatState = EBattleState.PlayerTurn;






    }

    private IEnumerator EndGame()
    {
        refUi.DisableButtonForfeit();
        string RandomWinningMessage()
        {
            string[] winningMessages = new[]
            {
                "The public approved you, you were musically excellent. You won the battle.",
                "Wow, you were really impressive, the public approves you! You won the battle.",
                "Where did you get this talent? You are musically a monster. The public has validated you hands down and you win the battle.",
                "Did you hear how the audience cheered for you? It's pure madness! The public approved you and you won the battle."
            };
            return winningMessages[Random.Range(0, winningMessages.Length)];
        }
        
        string RandomLoosingMessage()
        {
            string[] loosingMessages = new[]
            {
                "You were not at the top my boy, the public did not really like it. You lost the battle because the audience didn't approve of you.",
                "What the hell did you do? You were supposed to win this battle, not miss all your performances! You lost the battle because the public didn't approve of you.",
                "You really sucked, you were supposed to win. You lost the battle because the public didn't like you.",
                "You got lynched by the public so much. You lost because the public didn't approve of you."
            };
            return loosingMessages[Random.Range(0, loosingMessages.Length)];
        }
        
        
        switch (combatState)
        {
            case EBattleState.Won:
                Interface.SetDialogText(RandomWinningMessage());
                yield return new WaitForSeconds(5);
                SoundManager.Instance.PlayMusic();
                Destroy(gameObject);
                onCombatEnd(true, player.inspiration, player.resistance);
                break;
            case EBattleState.Lost:
                if (hasDeclareForfeit)
                    Interface.SetDialogText("You lost by forfeit.");
                else
                    Interface.SetDialogText(RandomLoosingMessage());
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
