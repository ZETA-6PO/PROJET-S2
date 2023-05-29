using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Q2", menuName = "Quest/Q2", order = 1)]
public class Q2 : Quest
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
        
        
        if (GameManager.Instance.IsInInventory("Fire Harmonica"))
        {
            if (sceneName == "ExtFirstScene")
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "Good! Now you really can start making some music.",
                            "In order to start your musical career, you must go to the youth center to do a " +
                            "demonstration in front of the children."
                        })

                    }),
                    Array.Empty<string>(),
                    i => { });
                Active = false;
                Completed = true;
                GameManager.Instance.AddItems(rewards);
                GameManager.Instance.quests[3].Active = true;
            }
        }
        else
        {
            if (sceneName == "ExtFirstScene")
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "With this money, you’ll be able to buy a Fire Harmonica at the music store, let’s check it out."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
            }
        }
        
        
        //Gestion des waypoints sur la MiniMap
        if (sceneName == "ExtFirstScene")
        {
            GameManager.Instance.isWaypointActive = true;
            if (GameManager.Instance.IsInInventory("Fire Harmonica"))
            {
                GameManager.Instance.displayedWaypoint = waypoints[1];
            }
            else
            {
                GameManager.Instance.displayedWaypoint = waypoints[0];
            }
        }
        else
        {
            GameManager.Instance.isWaypointActive = false;
        }
    
    }
    

}