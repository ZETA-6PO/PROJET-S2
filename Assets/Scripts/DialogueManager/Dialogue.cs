using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dialogue
{
    public Queue<SingleDialogue> dialogues;

    public Dialogue(SingleDialogue[] dialogues)
    {
        this.dialogues = new Queue<SingleDialogue>(); 
        foreach (var sd in dialogues)
        {
            this.dialogues.Enqueue(sd);
        }
    }
}
