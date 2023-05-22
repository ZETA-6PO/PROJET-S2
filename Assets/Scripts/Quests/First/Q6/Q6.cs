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

[CreateAssetMenu(fileName = "Q6", menuName = "Quest/Q6", order = 1)]

public class Q6 : Quest
{

    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        return;
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        return null;
    }


    public Vector3 momPosition;

    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            if (GameManager.Instance.coin >= 130 && GameManager.Instance.coin < 150)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Mum", new[]
                        {
                            "Go speak to your mum."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                if (GameManager.Instance.IsInInventory("Meet"))
                {
                    FindObjectOfType<parentsScript>().Enable(false, true, momPosition, Vector3.zero, 
                        () => {},
                        () => { }, () => { }, () =>
                        {
                            speakToMom2();
                            GameManager.Instance.RemoveItemName("Meet");
                        });
                }
                else
                {
                    FindObjectOfType<parentsScript>().Enable(false, true, momPosition, Vector3.zero, 
                        () => {} ,
                        () => { }, () => { }, () => speakToMom());
                }
                
            }
            FindObjectOfType<SceneController>().canGoOut = true;
        }
        

        if (sceneName == "IntFirstStudioScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "If you don't have the money don't come to me right now I'm very busy."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                SceneManager.LoadScene("ExtFirstScene");
                
        }

        if (sceneName == "ExtFirstScene")
        {
            if (GameManager.Instance.coin < 130)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Okay, 100 HypeCoins are missing to be able to collaborate with the boss of the music studio. " +
                            "A good way to earn this money is to perform at the youth center.",
                            "Maybe you can also do your parents a favor. Maybe they will give you a little something",
                            "Firstly, go the youth center!"
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
            
                GameManager.Instance.displayedWaypoint = waypoints[0];
            }
            else
            {
                if (GameManager.Instance.coin >= 130 && GameManager.Instance.coin < 150)
                {

                    if (GameManager.Instance.coin == 140)
                    {
                        FindObjectOfType<DialogManager>().StartDialogue(
                            new Dialogue(new[]
                            {
                                new SingleDialogue("", new[]
                                {
                                    "Go to the Butchery!"
                                })
                            }),
                            Array.Empty<string>(),
                            i => { });
                        GameManager.Instance.displayedWaypoint = waypoints[3]; // To go to the butcher
                    }
                    else
                    {
                        if (GameManager.Instance.IsInInventory("Meet"))
                        {
                            FindObjectOfType<DialogManager>().StartDialogue(
                                new Dialogue(new[]
                                {
                                    new SingleDialogue("", new[]
                                    {
                                        "Turn at Home!"
                                    })
                                }),
                                Array.Empty<string>(),
                                i => { });
                        }
                        else
                        {
                            FindObjectOfType<DialogManager>().StartDialogue(
                            new Dialogue(new[]
                            {
                                new SingleDialogue("", new[]
                                {
                                    "Now go see your parents I'm sure they can help you!"
                                })
                            }),
                            Array.Empty<string>(),
                            i => { });
                            
                        }
                        GameManager.Instance.displayedWaypoint = waypoints[1]; // To go to the parents House
                    }
                }

                if (GameManager.Instance.coin >= 150)
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("", new[]
                            {
                                "You should go back to the producer. I'm sure he'll agree to negotiate on the price."
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
            
                    GameManager.Instance.displayedWaypoint = waypoints[2];
                    Active = false;
                    Completed = true;
                    GameManager.Instance.quests[7].Active = true;
                }
            }
        }



        if (sceneName == "IntClubQuartierPoor" && GameManager.Instance.coin < 130)
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Go speak with the group."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            FindObjectOfType<GroupScript>().Enable(
                () => StartBattleDialogue(),
                () => { });
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

    private void speakToMom2() // After the butcher
    {
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Mum", new[]
                {
                    "Ohh thanks you son. This is for you!"
                })
            }),
            Array.Empty<string>(),
            i => { });
        GameManager.Instance.AddCoins(20);
    }


    private bool alreadySpeak;
    private void speakToMom() // Before the butcher
    {
        if (alreadySpeak)
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("Mum", new[]
                    {
                        "Go to the Butcher i wait you."
                    })
                }),
                Array.Empty<string>(),
                i =>
                { });
        }
        else
        {
            FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Mum", new[]
                {
                    "Ah my son you are here! Go and get me some meat from the butcher."
                })
            }),
            new string[]{"Ok!", "Ok!"},
            i =>
            {
                GameManager.Instance.AddCoins(10);
                alreadySpeak = true;
            });
            
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

    public Fighter boobsBas;

    private void StartBattle()
    {
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(boobsBas, isWin =>
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