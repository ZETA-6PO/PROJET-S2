using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Q15", menuName = "Quest/Q15", order = 1)]
public class Q15 : Quest
{
    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        had_pay = questProperties.First((property => property.name == "had_pay")).value == "1";
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        QuestData.QuestProperty _had_pay = new QuestData.QuestProperty()
        {
            name = "had_pay",
            value = had_pay ? "1" : "0"
        };
        return new[] { _had_pay };
    }

    public Vector3 producerPosition;
    public Vector3 freezPosition;
    public Fighter enemy;
    public Item toGive;
    public bool had_pay;
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
                    new SingleDialogue("Feneu", new[]
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
        if (GameManager.Instance.coin >= 500 || had_pay)
        {
            if (had_pay)
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
                            "Because you really think you have a chance against me? You're gonna pop then disapear like Desiigner."
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
                            "I must take you for some rap catechism, say less."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Well done ! By beating this artist, you earn 1000 HypeCoins and his instrument."
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
                GameManager.Instance.AddItems(rewards);
                GameManager.Instance.quests[16].Active = true;
                
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue(enemy.name, new[]
                        {
                            "Honestly easy I didn't even have to force it," + "even Lester and Fredo Santana can beat you."
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