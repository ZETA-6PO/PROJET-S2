using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks.Sources;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "Q0", menuName = "Quest/Q0", order = 1)]
public class Q0 : Quest
{

    [SerializeField] public Vector3 playerPosition;
    [SerializeField] public Vector3 momPosition;
    [SerializeField] public Vector3 dadPosition;
    
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
        Debug.Log($"Q0::onLoadScene(->'{sceneName}')");
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new []
                {
                    new SingleDialogue("", false, new []
                    {
                        "Welcom to hypepop",
                    })
                }),
                Array.Empty<string>(),
                i => {});
        }
        FindObjectOfType<PlayerController>().teleportPlayerAt(playerPosition);
        FindObjectOfType<parentsScript>().Enable(true, true, momPosition, dadPosition,
            () => speakToParents(),
                () => { }, () => { }, () => { });
    }

    
    /// <summary>
    /// This is the function called when the player enter the "parents" area.
    /// </summary>
    public void speakToParents()
    {
        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Mom", false, new[]
                {
                    "Hello son.",
                }),

                new SingleDialogue("Dad", false, new[]
                {
                    "Hello son.",
                }),
                new SingleDialogue("Mom", false, new[]
                {
                    "Bravo son, I am proud of you. You managed to beat the great champion of our city, that's very impressive! You have become a confirmed artist!",
                }),
            }),
            Array.Empty<string>(),
            i => { });
        Active = false;
        GameManager.Instance.quests[1].Active = true;
    }
    
}