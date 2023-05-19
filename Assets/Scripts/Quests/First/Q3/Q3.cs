using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Q3", menuName = "Quest/Q3", order = 1)]
public class Q3 : Quest
{
    private bool battle_finished;

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
        
        
        if (sceneName == "IntClubQuartierPoor")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("", new[]
                    {
                        "Now you have to go to the group to offer them a demonstration of your skills"
                    })
                }),
                Array.Empty<string>(),
                i => { });
            FindObjectOfType<GroupScript>().Enable(
                () => StartBattle(),
                () => { });
        }

    }

    private void StartBattle()
    {
        
    }
    

}