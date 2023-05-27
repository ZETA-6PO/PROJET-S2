using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Q12", menuName = "Quest/Q12", order = 1)]
public class Q12 : Quest
{

    public Vector3 mousePosition;

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
                        "The studio boss of this neighborhood has been impressed by your recent performances. " +
                        "He really wants to work with you and he demands no money for that. " +
                        "It’s a big opportunity for your career."
                    })
                }),
                Array.Empty<string>(),
                i => { });
            GameManager.Instance.displayedWaypoint = waypoints[0];
        }

        if (sceneName == "IntStudioMediumScene")
        {
            FindObjectOfType<MouseScript>().Enable(mousePosition, () => speakMouse());
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

    public Item toGive;
    private void speakMouse()
    {
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Mr.Anderson", new[]
                {
                    "As the rumors say, I’m very interested in working with you. " +
                    "If you want, I propose to you this beautiful instrument which can help you a lot for your next battles." +
                    "I also offer you some money which will allow you to develop your career faster.", "Now, make your own strange history"
                })
            }),
            Array.Empty<string>(),
            i => { });
        
        GameManager.Instance.AddCoins(100);
        GameManager.Instance.AddOneItem(toGive);
        Active = false;
        Completed = true;
        GameManager.Instance.quests[13].Active = true;
    }
}