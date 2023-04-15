using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Q2", menuName = "Quest/Q2", order = 1)]
public class Q2 : Quest
{
    
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
                            "Good! Now you really can start making some music."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                Active = false;
                Completed = false;
                //GameManager.Instance.quests[3].Active = true;
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
                            "With this money, you’ll be able to buy your first instrument at the music store, let’s check it out."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
            }
        }
        
    }
    

}