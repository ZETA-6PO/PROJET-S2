using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Q10", menuName = "Quest/Q10", order = 1)]
public class Q10 : Quest
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
                        "Go to the city and meet the producer, he is probably the person who can help you the most in your quest.",
                        "Your career isn’t the same anymore. Many people saw your performance and are interested in you. " +
                        "I have someone to present to you, who can help you a lot to develop your career faster."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0];
        }

        if (sceneName == "ExtSecondScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "This is the city! This is where your adventure must continue. " +
                        "Indeed, all the greatest artists known to date have come to this city " +
                        "to perform and try to demonstrate their talent.",
                        "But I must warn you. Many have failed and come out changed. But with hard work, determination " +
                        "and talent, I think you will be able to stand out among the greats in this industry.",
                        "The first thing you need to do now is to go meet the producer. In this town he is the only " +
                        "one who decides who has talent and who doesn't.",
                        "And don't forget, this city is full of new possibilities. " +
                        "For example, you can find better quality instruments in the music store."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[1];
            
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Go to speak at the producer."
                    })
                }),
                Array.Empty<string>(),
                i => { });

            FindObjectOfType<ProducerScript>().Enable(producerPosition, () => speakToProducer());
        }
        

            //Gestion Waypoints
        if (sceneName == "ExtFirstScene" || sceneName == "ExtSecondScene")
        {
            GameManager.Instance.isWaypointActive = true;
        }
        else
        {
            GameManager.Instance.isWaypointActive = false;
        }
    }

    private void speakToProducer()
    {
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Feneu", new[]
                {
                    "Hi. I introduce myself, i’m Feneu, and i'm an artist's producer from the city.", "I did Handball.",
                    "I saw your performance beat the best artist in your neighborhood and i’m interested in you.",
                    "I know everybody here, so I can find you many contacts in order to develop your career in a " +
                    "better war. But for that, you have to earn my truth."
                }),
                new SingleDialogue("", new[]
                {
                    "To gain his truth, you’ll have to beat every artist he offers you to face."
                }),
                new SingleDialogue("Feneu", new[]
                {
                    "There is a first challenge for you. Beat this artist to prove to me that you are competent " +
                    "enough to be an artist."
                }),

            }),
            new string[]{"Let's Go!", "I prefer to postpone it until later..."},
            i =>
            {
                if (i == 1)
                {
                    StartBattleDialogue();
                }
                else
                {
                    SceneManager.LoadScene("ExtSecondScene");
                    GameManager.Instance.displayedWaypoint = waypoints[1];
                }
            });
        
    }

    public Fighter enemy;
    private void StartBattleDialogue()
        {
            //Dialogue before Battle
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue(enemy.name, new[]
                    {
                        "Because you really think you have a chance against me? I don't even know why the producer " +
                        "agreed to work with someone like you. 2.7 2.7 ", "I'm gonna break your bones and drink your blood"
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
                                "I will never get over such humiliation. You are truly a great artist.", 
                                "Come to Sevran when you have time."
                            }),
                            new SingleDialogue("Studio Boss", new[]
                            {
                                "You might have been lucky but hey. Here you go as promised."
                            }),
                            new SingleDialogue("", new[]
                            {
                                "Good job ! Here are 200 HypeCoins offered by the Producer to thank you for this performance."
                            })
                        }),
                        Array.Empty<string>(),
                        i => { });
                    FindObjectOfType<GameManager>().AddCoins(200);
                    Active = false;
                    Completed = true;
                    GameManager.Instance.AddItems(rewards);
                    GameManager.Instance.quests[11].Active = true;
                    
                }
                else
                {
                    FindObjectOfType<DialogManager>().StartDialogue(
                        new Dialogue(new[]
                        {
                            new SingleDialogue(enemy.name, new[]
                            {
                                "Honestly easy I didn't even have to force it.", "Even the worst musician of Sevran could beat you."
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