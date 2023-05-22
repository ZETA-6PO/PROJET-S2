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

[CreateAssetMenu(fileName = "Q7", menuName = "Quest/Q7", order = 1)]

public class Q7 : Quest
{

    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        return;
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        return null;
    }

    

    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<SceneController>().canGoOut = true;
        }
        

        if (sceneName == "IntFirstStudioScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "There you are! But you don't have the money. Well, " +
                            "I'm willing to make an exception for you since you are a promising talent. ",
                            "You were quick ! I’m really happy to start a new collaboration with you. " +
                            "In order to start well, I offer you this instrument. I hope this can help you in the future.",
                            "Also, I present to you a friend : he is the best artist in the neighborhood. " +
                            "If you beat him, you’ll be able to go even further, but everything in its time. ",
                            "Try to beat him when you're really ready, and when you have the money, " +
                            "because you can face him for 300 HypeCoins, and 50 HypeCoins if you lose the first time."
                        }),
                        new SingleDialogue("KoBalaD", new[]
                        {
                            "There you are! But you don't have the money. Well, " +
                            "I'm willing to make an exception for you since you are a promising talent. ",
                            "You were quick ! I’m really happy to start a new collaboration with you. " +
                            "In order to start well, I offer you this instrument. I hope this can help you in the future.",
                            "Also, I present to you a friend : he is the best artist in the neighborhood. " +
                            "If you beat him, you’ll be able to go even further, but everything in its time. ",
                            "Try to beat him when you're really ready, and when you have the money, " +
                            "because you can face him for 300 HypeCoins, and 50 HypeCoins if you lose the first time."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });

        }

        if (sceneName == "ExtFirstScene")
        {
            GameManager.Instance.displayedWaypoint = waypoints[0]; // To go to the Music Studio
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
                new SingleDialogue("Sam", new[]
                {
                    "Ah you're still here. So how did it go with Studio Boss? I'm sure he offered you to work with him, " +
                    "am I right? Anyway, when he finds a talent he never makes a mistake.",
                    " He has a good eye for this kind of thing. Do you want to have a duel? " +
                    "There are 30 HypeCoins in play if you want. ",
                    "But this time you won't be facing me, but Boobsbas. And you'll see it won't be the same."
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

    public AttackObject[] attackBoobsBas;
    
    private void StartBattle()
    {
        Fighter ennemy = new Fighter("Boobs Bas", 70, 40, attackBoobsBas);
        
        
        
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(ennemy, isWin =>
        {
            if (isWin)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("BoobsBas", new[]
                        {
                            "How can the Duke, the Prince, the Phoenix who has been around for years like me lose to " +
                            "a youngster like you? Take care of yourself. See you at Orly Airport. " +
                            "We'll see if you're still smart."
                        }),
                        new SingleDialogue("Sam", new[]
                        {
                            "Congratulations, it's really impressive what you've done! Here you go to say thank you."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Good job ! Here are 30 HypeCoins offered by the young Center to thank you for this performance."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                FindObjectOfType<GameManager>().AddCoins(30);
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Boobs Bas", new[]
                        {
                            "At the same time it is logical that you lose. I am a much better artist. " +
                            "But if you want to come back, I'll be happy to beat you up again."
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