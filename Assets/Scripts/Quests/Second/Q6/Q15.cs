using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Q15", menuName = "Quest/Q15", order = 1)]
public class Q15 : Quest
{

    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        return;
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        return null;
    }

    public Vector3 producerPosition;
    public Vector3 freezPosition;
    public Fighter enemy;
    public Item toGive;
    public bool hadPaid;
    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<SceneController>().canGoOut = true;
        }

        if (sceneName == "ExtFirstScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "You should return to the city to continue your adventure but if you have things to " +
                        "do here you are free to wander."
                    })
                }),
                Array.Empty<string>(),
                i => { });
        }

        if (sceneName == "ExtSecondScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "What a journey ! Firstable, congratulations on making it this far. " +
                        "Now you have to beat the last artist of the arena. " +
                        "It's the best, the one who has never been beaten by anybody here. " +
                        "I advise you to be as prepared as you can."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0];
        }
        

        if (sceneName == "IntQGMusicScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("Producer", new[]
                    {
                        "I saw all your battles and you did a good job. Now it’s time to really prove to me what you can do. " +
                        "It’s not the difficulty you’ve had so far. Beat him and I’ll take your career even further."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            
            FindObjectOfType<ProducerScript>().Enable(producerPosition, () => {});
            FindObjectOfType<Freez>().Enable(freezPosition, () => StartBattleDialogue());

        }
        

            //Gestion Waypoints
        if (sceneName == "ExtSecondScene")
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
        if (GameManager.Instance.coin >= 500 || hadPaid)
        {
            if (hadPaid)
            {
                if (GameManager.Instance.coin >= 100)
                {
                    GameManager.Instance.AddCoins(-100);
                    StartBattleDialogue();
                }
                else
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue("", new[]
                            {
                                "Come back when you have enough money. (100 HypeCoins)"
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    SceneManager.LoadScene("ExtSecondScene");
                }
            }
            else
            {
                GameManager.Instance.AddCoins(-500);
                //Dialogue before Battle
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue(enemy.name, new[]
                        {
                            "Because you really think you have a chance against me? I don't even know why the producer " +
                            "agreed to work with someone like you. "
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
        }
        else
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Come back when you have enough money. (500 HypeCoins)"
                    })
                }),
                Array.Empty<string>(),
                i => { });
            SceneManager.LoadScene("ExtSecondScene");
        }
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
                        new SingleDialogue(enemy.name, new[]
                        {
                            "I will never get over such humiliation. You are truly a great artist."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Well done ! By beating this artist, you earn 100 HypeCoins and his instrument."
                        }),
                        new SingleDialogue("Producer", new[]
                        {
                            "I saw your battle, I’m impressed by your job, continue like it."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                GameManager.Instance.AddCoins(1000);
                
                GameManager.Instance.AddOneItem(toGive);
                Active = false;
                Completed = true;
                //GameManager.Instance.quests[16].Active = true;
                
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue(enemy.name, new[]
                        {
                            "Honestly easy I didn't even have to force it."
                        })
                    }),
                    new string[]{"I will beat you this time!", "No I prefer to stop the massacre..."},
                    i =>
                    {
                        if (i == 1)
                        {

                            StartBattle(enemy);
                        }
                        else
                        {
                            SceneManager.LoadScene("ExtSecondScene");
                        }
                    });
            }
        } );
    }
        
}