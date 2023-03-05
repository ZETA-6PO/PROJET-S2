using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxFirstDialog : MonoBehaviour
{
    public DialogManager refDialogManager;
    private Dialogue _dialogue;
    
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        _dialogue = new Dialogue(new []
        {
            new SingleDialogue("Mom", false, new []
            {
                "Hello son !"
            }),
            new SingleDialogue("Dad", false, new []
            {
                "Hey Son !"
            }),
            new SingleDialogue("Mom", false, new []
            {
                "I need bread for lunch. Could you bring me one, here is 10 HypCoin !",
                "You can go to the shop at the end of the town."
            })
        });
        //on fais un appel au DialogManager.
        refDialogManager.StartDialogue(_dialogue);
    }
}
