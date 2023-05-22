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

[CreateAssetMenu(fileName = "Q8", menuName = "Quest/Q8", order = 1)]

public class Q8 : Quest
{

    [SerializeField] private Vector3 momPosition;
    [SerializeField] private Vector3 dadPosition;
    
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
            FindObjectOfType<parentsScript>().Enable(true, true, momPosition, dadPosition,
                () => speakToParents(),
                () => { }, () => { }, () => { });
            
            
            FindObjectOfType<SceneController>().canGoOut = true;
        }
        

        if (sceneName == "IntFirstStudioScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "Don't come back until you have the money! " +
                            "Besides, I need the agreement of your parents because you are a minor."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });

        }

        if (sceneName == "ExtFirstScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Now you have accomplished a lot, it’s time to tell your parents, I think they’ll be really happy to hear that.",
                        "Go to your home and approach your parents."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0]; // To ParentsHouse
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

    private void speakToParents()
    {
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Dad", new[]
                {
                    "Wow ! It’s incredible, we are so happy for you son !"
                }),
                new SingleDialogue("Mum", new[]
                {
                    "Yes, it’s fabulous. We want to help you even more. " +
                    "Here are 100 HypeCoins to continue in this way ! We trust you completely."
                })
            }),
            Array.Empty<string>(),
            i => { });
        GameManager.Instance.AddCoins(100);
        Active = false;
        Completed = true;
        GameManager.Instance.quests[9].Active = true;
    }
}