using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Q14", menuName = "Quest/Q14", order = 1)]
public class Q14 : Quest
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
    public Vector3 valdPosition;
    public Vector3 plaiboyPosition;
    public Vector3 laylowPosition;
    public Fighter enemyVald;
    public Fighter enemyLaylow;
    public Fighter enemyPlaiboy;
    public Item toGiveLaylow;
    public Item toGivePlaiboy;
    public Item toGiveVald;
    private bool winP;
    private bool winV;
    private bool winL;
    
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
            if (winL && winP && winV)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Congratulations on beating them all !"
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                
                Active = false;
                Completed = true;
                GameManager.Instance.quests[15].Active = true;
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Go to the battle arena in order to see who you would face."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                GameManager.Instance.displayedWaypoint = waypoints[0];
            }
        }
        

        if (sceneName == "IntQGMusicScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Now you have beaten the first artist, you have to beat them all to earn the producer’s trust."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            
            FindObjectOfType<ProducerScript>().Enable(producerPosition, () => {});
            FindObjectOfType<Vald>().Enable(valdPosition, () => StartBattleDialogue(enemyVald));
            FindObjectOfType<Laylow>().Enable(laylowPosition, () => StartBattleDialogue(enemyLaylow));
            FindObjectOfType<Plaiboy>().Enable(plaiboyPosition, () => StartBattleDialogue(enemyPlaiboy));

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
    
    
    private void StartBattleDialogue(Fighter enemy)
        {
            if (GameManager.Instance.coin >= 100)
            {
                GameManager.Instance.AddCoins(-100);
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
                            StartBattleDialogue(enemy);
                        }
                    });
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "To take on each artist you need 100 HypeCoins. Come back when you've got enough."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
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
                    GameManager.Instance.AddCoins(100);
                    if (enemy.name == "Vlad")
                    {
                        GameManager.Instance.AddOneItem(toGiveVald);
                        winV = true;
                    }
                    if (enemy.name == "Plaiboy Carti")
                    {
                        GameManager.Instance.AddOneItem(toGivePlaiboy);
                        winP = true;
                    }
                    if (enemy.name == "Laylow")
                    {
                        GameManager.Instance.AddOneItem(toGiveLaylow);
                        winL = true;
                    }
                    Active = false;
                    Completed = true;
                    GameManager.Instance.quests[14].Active = true;
                    
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