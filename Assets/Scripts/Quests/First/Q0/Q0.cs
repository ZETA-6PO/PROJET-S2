using System;
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
        if (sceneName == "IntFirstHouseScene")
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new []
                {
                    new SingleDialogue("", new []
                    {
                        "Welcom to hypepop !", "You always wanted to become a famous artist and the time has come !",
                        "From today, you will do everything that you can to grow in the world of music.",
                        "However, you are still living with your parents, so you need to help them sometimes, even more if you want them to help you.",
                        "Oh ! They call you to do them a favor, go see them."
                    })
                }),
                Array.Empty<string>(),
                i => {});
            FindObjectOfType<PlayerController>().teleportPlayerAt(playerPosition);
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
        if (Active)
        {
            FindObjectOfType<DialogManager>().StartDialogue(
                new Dialogue(new[]
                {
                    new SingleDialogue("Dad", new[]
                    {
                        "Hi son ! Me and your mother need you.",
                    }),

                    new SingleDialogue("Mum", new[]
                    {
                        "Yes,we want you to buy us some bread.",
                        "Here’s 10 HypeCoins for it. You could keep the money and you might get a reward after this favor."
                    }),
                    new SingleDialogue("", new[]
                    {
                        "Go to the bakery and buy bread for your parents. Then bring it to them.",
                    }),
                }),
                Array.Empty<string>(),
                i => { });
            
            GameManager.Instance.AddCoins(10);
            FindObjectOfType<SceneController>().canGoOut = true;
            Active = false;
            Completed = true;
            GameManager.Instance.quests[1].Active = true;
        }
        Debug.Log($"Q0 is active : {Active}");
        Debug.Log($"Q1 is Active : {GameManager.Instance.quests[1].Active}");
    }
    
}