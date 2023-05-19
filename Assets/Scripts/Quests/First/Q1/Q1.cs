using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Q1", menuName = "Quest/Q1", order = 1)]
public class Q1 : Quest
{
    
    [SerializeField] public Vector3 momPosition;
    [SerializeField] public Vector3 dadPosition;
    [SerializeField] public Vector3 playerSpawn;

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
        Debug.Log(sceneName);
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<SceneController>().canGoOut = true;
            Debug.Log("itWorks");
            FindObjectOfType<parentsScript>().Enable(true, true, momPosition, dadPosition,
                () => speakToParents(),
                () => { }, () => { }, () => { });
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
            GameManager.Instance.AddCoins(20);
            Active = false;
            Completed = true;
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