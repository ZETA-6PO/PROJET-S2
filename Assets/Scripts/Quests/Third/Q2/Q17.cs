using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Q17", menuName = "Quest/Q17", order = 1)]
public class Q17 : Quest
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
    public Vector3 bossPosition;
    public Fighter boss;
    public Item toGive;
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
                        "You should head to the rich district to take on this city's greatest artist from now on."
                    })
                }),
                Array.Empty<string>(),
                i => { });
        }

        if (sceneName == "ExtLastScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "You should head to the rich district to take on this city's greatest artist from now on."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0];
        }
        

        if (sceneName == "IntBossHouse")
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
            FindObjectOfType<bossScript>().Enable(bossPosition, () => StartBattleDialogue());


        }
        

            //Gestion Waypoints
        if (sceneName == "ExtSecondScene" || sceneName == "ExtLastScene")
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
                new SingleDialogue(boss.name, new[]
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
                    StartBattle(boss);
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
                        new SingleDialogue(enemy.name, new[]
                        {
                            "I will never get over such humiliation. You are truly a great artist."
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
                            SceneManager.LoadScene("ExtLastScene");
                        }
                    });
            }
        } );
    }
        
}