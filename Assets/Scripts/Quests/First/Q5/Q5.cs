using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Q5", menuName = "Quest/Q5", order = 1)]

public class Q5 : Quest
{

    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        return;
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        return null;
    }

    public Vector3 positionLuv_Restla;
    
    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<SceneController>().canGoOut = true;
        }

        if (sceneName == "IntFirstStudioScene")
        {
            FindObjectOfType<Luv_Restla>().TeleportAt(positionLuv_Restla);
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Enter the building and approach the boss."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            StartBattleDialogue();
        }

        if (sceneName == "ExtFirstScene")
        {
            if (!Win)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Come back here when you feel ready!"
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
            }
            GameManager.Instance.displayedWaypoint = waypoints[0];
        }
        

        //Gestion Waypoints
        if (sceneName == "ExtFirstScene")
        {
            GameManager.Instance.isWaypointActive = true;
        }
        else
        {
            GameManager.Instance.isWaypointActive = false;
        }

    }
    

    private void StartBattleDialogue()
    {
        //Dialogue before Battle
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Studio Boss", new[]
                {
                    "I understand that you would like to work on your music with me. I’m not against this idea, " +
                    "however not everyone can work with me like that, you have to bring me some money. " +
                    "I ask you for 200 HypeCoins.",
                    "To help you, I can offer you a battle against a man who often comes here. " +
                    "If you win this battle, you’ll earn 100 HypeCoins which will help you to pay me."
                })
            }),
            new string[]{"Let's Go!", "Can you repeat ?"},
            i =>
            {
                if (i == 1)
                {
                    StartBattle();
                }
                else
                {
                    StartBattleDialogue();
                }
            });
    }

    public AttackObject[] attackLuv_Restla;
    public bool Win = false;
    private void StartBattle()
    {
        Fighter ennemy = new Fighter("Luv Restla", 50, 200, attackLuv_Restla);
        
        
        
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(ennemy, isWin =>
        {
            if (isWin)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Luv Restla", new[]
                        {
                            "Noo! How is it possible that an artist of my quality loses to a stranger like you! " +
                            "You were lucky! You don't lose anything by waiting. " +
                            "Anyway it's only temporary, you can be sure that I will take my revenge!"
                        }),
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "Wow ! I appreciate your talent, it will be a pleasure to work with you when you " +
                            "have enough money to pay me."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Good job ! Here are 100 HypeCoins offered by the Studio Boss to thank you for this performance."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                FindObjectOfType<GameManager>().AddCoins(100);
                Win = true;
                
                Active = false;
                Completed = true;
                GameManager.Instance.quests[6].Active = true;
                
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Luv Restla", new[]
                        {
                            "Ah! Ah! I knew you just couldn't beat me! But if you want to continue to be humiliated you can try again."
                        })
                    }),
                    new string[]{"I will beat you this time!", "No I prefer to stop the massacre..."},
                    i =>
                    {
                        if (i == 1)
                        {
                            StartBattle();
                        }
                        else
                        {
                            SceneManager.LoadScene("ExtFirstScene");
                        }
                    });
            }
        } );
        
    }
    
}