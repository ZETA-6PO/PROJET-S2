using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Q1", menuName = "Quest/Q1", order = 1)]
public class Q1 : Quest
{
    
    [SerializeField] public Vector3 momPosition;
    [SerializeField] public Vector3 dadPosition;


    private bool test;

    public override void LoadQuestProperties(QuestData.QuestProperty[] questProperties)
    {
        test = questProperties.First((property => property.name == "test")).value == "1";
    }

    public override QuestData.QuestProperty[] SaveQuestProperties()
    {
        QuestData.QuestProperty _test = new QuestData.QuestProperty()
        {
            name = "test",
            value = test ? "1" : "0"
        };
        return new[] { _test };
    }

    public override void OnLoadScene(string sceneName)
    {
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<parentsScript>().Enable(true, true, momPosition, dadPosition,
                () => speakToParents(),
                () => { }, () => { }, () => { });
        }

        if (sceneName == "ExtFirstScene")
        {
            GameManager.Instance.isWaypointActive = true;
            if (GameManager.Instance.IsInInventory("Bread"))
            {
                GameManager.Instance.displayedWaypoint = waypoints[1];
                test = true;
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

    
    /// <summary>
    /// This is the function called when the player enter the "parents" area.
    /// </summary>
    public void speakToParents()
    {
        if (GameManager.Instance.IsInInventory("Bread"))
        {
            GameManager.Instance.RemoveItemName("Bread");
            
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("Dad", new[]
                    {
                        "Thanks son ! For helping us, we give you some money which could help you. Here’s 20 Hype Coins, make good use of them my boy."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            if (GameManager.Instance.coin != 20)
            {
                int to_give = 20 - GameManager.Instance.coin;
                GameManager.Instance.AddCoins(to_give);
            }
            Active = false;
            Completed = true;
            GameManager.Instance.AddItems(rewards);
            GameManager.Instance.quests[2].Active = true;
            
        }
        else
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("Mom", new[]
                    {
                        "You should first bring us some bread",
                    })
                }),
                Array.Empty<string>(),
                i => { });
        }
        
        FindObjectOfType<SceneController>().canGoOut = true;
    }
    
}