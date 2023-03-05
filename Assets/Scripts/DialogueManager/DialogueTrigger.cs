using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Animator animator;

    public void TriggerDialogue()
    {
        animator.SetBool("IsOpen", false);
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }
}
