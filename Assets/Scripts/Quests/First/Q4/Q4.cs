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

[CreateAssetMenu(fileName = "Q4", menuName = "Quest/Q4", order = 1)]

public class Q4 : Quest
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
        
        if (sceneName == "ExtFirstScene")
        {
            if (GameManager.Instance.IsInInventory("Foam Drum"))
            {
                GameManager.Instance.displayedWaypoint = waypoints[1];
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Now you have a little bit of stuff and experience you can go to the neighborhood music " +
                            "studio in order to meet the studio boss."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                Active = false;
                Completed = true;
                GameManager.Instance.quests[5].Active = true;
            }
            else
            {
                GameManager.Instance.displayedWaypoint = waypoints[0];
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "With these HypeCoins, you can buy a new instrument available at the music store. " +
                            "It will be necessary to progress in your musical career.",
                            "Go see what it’s all about."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
            }
        }

        if (sceneName == "IntMusicShopPoor")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Now go the buy a Foam Drum it will help you for the next step."
                    })
                }),
                Array.Empty<string>(),
                i => { });
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