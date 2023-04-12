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
    
    public void tg()
    {
        Debug.Log("tg");
    }
    
    public override void onLoadScene(string sceneName)
    {
        Debug.Log($"Q0::onLoadScene(->'{sceneName}')");
        FindObjectOfType<PlayerController>().teleportPlayerAt(playerPosition);
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
        FindObjectOfType<parentsScript>().Enable(true, true, momPosition, dadPosition,
            () =>
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new []
                    {
                        new SingleDialogue("Mom", false, new []
                        {
                            "Hello son.",
                        }),
                        
                        new SingleDialogue("Dad", false, new []
                        {
                            "Hello son.",
                        }),
                        new SingleDialogue("Mom", false, new []
                        {
                            "Ta mere la pute va cherche du pain.",
                        }),
                    }),
                    Array.Empty<string>(),
                    i => {});
                QuestManager.Instance.OnCompleteQuest(QuestId);
                QuestManager.Instance.OnActivateQuest("Q1");
            },
            () => {},
            () => {},
            () => {}
            );
    }

    public override IEnumerator onStartQuest()
    {
        throw new NotImplementedException();
    }
    
    
}