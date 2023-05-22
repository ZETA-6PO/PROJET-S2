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

    [SerializeField] public Item item;
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
            if (GameManager.Instance.coin >= 150)
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
                            "I'm going to blow you away without a care in the world!"
                        }),
                        new SingleDialogue("", new []
                        {
                            "Great, you now have a new instrument available during battles. " +
                            "Don’t hesitate to use it in order to win more often." +
                            "Furthermore, there are some new instruments available in the music store,check them out. " +
                            "They can really help you in the future."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                GameManager.Instance.AddCoins(-150);
                GameManager.Instance.AddOneItem(item);
                Active = false;
                Completed = true;
                GameManager.Instance.quests[8].Active = true;

            }
        }

        if (sceneName == "ExtFirstScene")
        {
            GameManager.Instance.displayedWaypoint = waypoints[0]; // To go to the Studio
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

}
