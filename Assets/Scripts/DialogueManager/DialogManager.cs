using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator dialogAnimator;
    public Animator startConversationAnimator;
    public Queue<string> sentences;
    public Queue<SingleDialogue> singleDialogues;

    public void StartDialogue(Dialogue dialogue)
    {
        singleDialogues = dialogue.dialogues;
        sentences = new Queue<string>();
        dialogAnimator.SetBool("IsOpen", true);
        SingleDialogue sd = singleDialogues.Dequeue();
        nameText.text = sd.name;
        DisplayNextDialogue(sd);
    }

    public void DisplayNextDialogue(SingleDialogue sd)
    {
        sentences.Clear();
        foreach (var sentece in sd.senteces)
        {
            sentences.Enqueue(sentece);
        }
        nameText.text = sd.name;
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            if (singleDialogues.Count != 0)
            {
                DisplayNextDialogue(singleDialogues.Dequeue());
            }
            else
            {
                EndDialogue();
            }
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
        dialogAnimator.SetBool("IsOpen", false);
        startConversationAnimator.SetBool("IsOpen", false);
    }

}
