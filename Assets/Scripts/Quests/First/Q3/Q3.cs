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

[CreateAssetMenu(fileName = "Q3", menuName = "Quest/Q3", order = 1)]

public class Q3 : Quest
{

    private bool Win = false;
    [SerializeField] public Fighter sam;

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
                () => StartBattleDialogue(),
                () => { });
        }

        if (Win)
        {
            if (sceneName == "ExtFirstScene")
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("", new[]
                        {
                            "With these HypeCoins, you can buy a new instrument available at the music store. " +
                            "It will be necessary to progress in your musical career. Go see what it’s all about!"
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                Active = false;
                Completed = true;
                //GameManager.Instance.quests[4].Active = true;
            }
        }

    }

    private void StartBattleDialogue()
    {
        // Dialogue before Combat

        FindObjectOfType<DialogManager>().StartDialogue(
            new Dialogue(new[]
            {
                new SingleDialogue("Children", new[]
                {
                    "Ooh! Hi, are you new here?"
                }),
                new SingleDialogue("Sam", new[]
                {
                    "Great, my name is Sam and I've been in this group for 2 years. I'll tell you how we operate here. ",
                    "Every week we write our songs at home. Then once we get to our meeting here, we compete in a 1 on 1 format. ",
                    "Whoever wins gets the prize money. Since it's your first time, I suggest that I put money on the line for both of us. " +
                    "If you win, you get the money. Are you ready? Let's get started!"
                }),
                new SingleDialogue("", new[]
                {
                    "Now you're going to duel with Sam. The music is about to start so remember to turn up the sound on " +
                    "your computer. Once the music starts, all you have to do is press the ask key on your keyboard. " +
                    "Good luck to you!"
                })
            }),
            Array.Empty<string>(),
            i => { });
        
        StartBattle();
    }

    private void StartBattle()
    {
        Fighter ennemy = new Fighter("Sam", 20, 20, new AttackObject[]{});
        
        
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(ennemy, isWin =>
        {
            if (isWin)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Sam", new[]
                        {
                            "Wow you are really well done! I didn't expect to be beaten by a newbie... " +
                            "I still have to practice. As for you, you seem to have a lot of talent. ",
                            "You really need to come back often. We can get you to meet Mike. Mike is the " +
                            "best artist in our club. You know he works directly with a producer in town. " +
                            "Maybe one day you'll get to play him.",
                            "Ahh! Here's your reward for beating me. Here's 20 HypeCoins for you!"
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Good job ! Here are 20 HypeCoins offered by the children to thank you for this performance."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                FindObjectOfType<GameManager>().AddCoins(20);
                Win = true;
            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("Sam", new[]
                        {
                            "Don't worry about it, it happens to everyone. Let's try again, I'm sure you'll have better luck this time."
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });
                StartBattle();
            }
        } );
        
    }

    
}