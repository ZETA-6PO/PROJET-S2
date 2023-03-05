using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerClose : MonoBehaviour
{
    public Animator animator;
    
    public void TriggerDialogue()
    {
        animator.SetBool("IsOpen",false); 
    }
}
