using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Q0 : MonoBehaviour
{
    public DialogManager refDialogManager;
    private Dialogue _dialogue;
    public IEnumerator demarre()
    {
        yield return new WaitUntil(() => refDialogManager!=null);
        _dialogue = new Dialogue(new []
        {
            new SingleDialogue("", true, new[]
            {
                "Welcome to Hypepop !",
                "You will soon live the most beautiful adventure. For this you will need several things that I will introduce to you now. ",
                "The first one is your HypeCoins. They will be very useful to advance in your quests and achieve your goal. To become the greatest artist of all time.",
                "You will also have access to your inventory in which you can store all your items and access the statistics of your items. This will be very useful but you will find out later.",
                "Ahhh! But by the way, I didn't introduce myself! I am the narrator. I will help you throughout your adventure to achieve your goals. By the way, you should go talk to your parents. I think they have something to tell you."
            })
        });
        
        refDialogManager.StartDialogue(_dialogue, new string[]{}, (i) =>
        {
            DataPersistenceManager.Instance.gameData.HasDoneQ0 = true;
            SceneManager.LoadScene("IntFirstHouseScene");
        });
    }
}
