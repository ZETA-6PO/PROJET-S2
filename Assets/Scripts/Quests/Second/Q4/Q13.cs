using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Q13", menuName = "Quest/Q13", order = 1)]
public class Q13 : Quest
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
    public Vector3 damsoPosition;
    public Fighter enemy;
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
                        "Now you are well stuffed, go to the battle arena in order to see who you would face."
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
                    new SingleDialogue("", new[]
                    {
                        "It’s here where you will face every artist of this district. " +
                        "To unlock the next district, you must beat every artist who is here at least once.",
                        "Okay, it’s time to face the first artist of this arena, every time you win against " +
                        "an artist you’ve never beaten, you win not only money but also your opponent’s instrument." +
                        "Let’s try to face the first artist."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            
            FindObjectOfType<ProducerScript>().Enable(producerPosition, () => {});
            FindObjectOfType<Damso>().Enable(damsoPosition, () => StartBattleDialogue());

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
                    
                    GameManager.Instance.AddOneItem(toGive);
                    Active = false;
                    Completed = true;
                    GameManager.Instance.AddItems(rewards);
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