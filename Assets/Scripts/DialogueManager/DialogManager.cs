using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public Queue<string> sentences; 
    
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        nameText.text = dialogue.name;

        foreach (string sentence in dialogue.senteces)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSenetence(sentence));

    }

    IEnumerator TypeSenetence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            
            yield return new WaitForSeconds(0.025f);
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}
