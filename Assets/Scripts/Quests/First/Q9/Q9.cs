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

[CreateAssetMenu(fileName = "Q9", menuName = "Quest/Q9", order = 1)]

public class Q9 : Quest
{
    
    
    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        return;
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        return null;
    }

    public bool had_pay;
    public Fighter enemy;
    [SerializeField] public Item item;

    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<SceneController>().canGoOut = true;
        }
        

        if (sceneName == "IntFirstStudioScene")
        {
            if (!had_pay)
            {
                if (GameManager.Instance.coin >= 300)
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("Studio Boss", new[]
                            {
                                "There you are! Do you have the money? Ok then let the battle begin!"
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    GameManager.Instance.AddCoins(-300);
                    StartBattleDialogue();
                }
                else
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("Studio Boss", new[]
                            {
                                "Don't come back until you have the money!"
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    SceneManager.LoadScene("ExtFirstScene");

                }   
            }
            else
            {
                if (GameManager.Instance.coin >= 50)
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("Studio Boss", new[]
                            {
                                "There you are! Do you have the money? Ok then let the battle begin!"
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    GameManager.Instance.AddCoins(-50);
                    StartBattleDialogue();
                }
                else
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("", new[]
                            {
                                "Mmmh... You don't have enough money. Come back when you have more " +
                                "than 50 HypeCoins."
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    SceneManager.LoadScene("ExtFirstScene");
                }
            }

        }

        if (sceneName == "ExtFirstScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Now you are fully equipped to beat the best artist of the neighborhood who you met at the music studio." +
                        "But before that, you need to collect 300 Hype Coins in order to face him. Earn money by performing at the youth center."
                    }),
                    new SingleDialogue("", new[]
                    {
                        "You can also perform with other artists in town to make more money and buy more instruments in " +
                        "the Music Shop. If you buy new ones, don't forget to equip them in your inventory."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0]; // To Studio
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
                new SingleDialogue("KoBalaD", new[]
                {
                    "Because you really think you have a chance against me? I don't even know why the producer " +
                    "agreed to work with someone like you."
                })
            }),
            new string[]{"Let's Go!", "Can you repeat ?"},
            i =>
            {
                if (i == 1)
                {
                    StartBattle(enemy);
                }
                else
                {
                    StartBattleDialogue();
                }
            });
    }
    

    private void StartBattle(Fighter enemy)
    {
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(enemy, isWin =>
        {
            if (isWin)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("KoBalaD", new[]
                        {
                            "I will never get over such humiliation. You are truly a great artist."
                        }),
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "You might have been lucky but hey. Here you go as promised."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Good job ! Here are 1000 HypeCoins offered by the Studio Boss to thank you for this performance.",
                            "Great, you now have a new instrument available during battles. Don’t hesitate to use it in order to win more often.",
                            "Furthermore, there are some new instruments available in the music store,check them out. They can really help you in the future."
                            
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                FindObjectOfType<GameManager>().AddCoins(1000);
                GameManager.Instance.AddOneItem(item);
                Active = false;
                Completed = true;
                //GameManager.Instance.quests[10].Active = true;
                
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("KoBalaD", new[]
                        {
                            "Honestly easy I didn't even have to force it."
                        })
                    }),
                    new string[]{"I will beat you this time!", "No I prefer to stop the massacre..."},
                    i =>
                    {
                        if (i == 1 && GameManager.Instance.coin >= 50)
                        {
                            GameManager.Instance.AddCoins(-50);
                            StartBattle(enemy);
                        }
                        else
                        {
                            if (i == 1)
                            {
                                FindObjectOfType<DialogManager>().StartDialogue(
                                    new Dialogue(new[]
                                    {
                                        new SingleDialogue("", new[]
                                        {
                                            "Mmmh... You don't have enough money. Come back when you have more " +
                                            "than 50 HypeCoins."
                                        })
                                    }),
                                    Array.Empty<string>(),
                                    i => { });
                                SceneManager.LoadScene("ExtFirstScene");
                            }
                            else
                            {
                                SceneManager.LoadScene("ExtFirstScene");   
                            }
                        }
                    });
            }
        } );
        
    }
}