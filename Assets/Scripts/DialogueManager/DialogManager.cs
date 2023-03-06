using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI button1;
    public TextMeshProUGUI button2;
    public Animator dialogAnimator;
    public Animator startConversationAnimator;
    public Queue<string> sentences;
    public Queue<SingleDialogue> singleDialogues;
    private Action<int> onResponse;
    private bool hasResponse;


    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }
    public void StartDialogue(Dialogue dialogue, string[] responses, Action<int> OnResponse)
    {
        hasResponse = false;
        if (responses.Length != 0)
            hasResponse = true;
        playerController.canMove = false;
        onResponse = OnResponse;
        if (responses.Length != 0)
        {
            button1.text = responses[0];
            button2.text = responses[1];  
        }
        singleDialogues = dialogue.dialogues;
        sentences = new Queue<string>();
        dialogAnimator.SetBool("IsOpen", true);
        SingleDialogue sd = singleDialogues.Dequeue();
        DisplayNextDialogue(sd);
    }

    public void DisplayNextDialogue(SingleDialogue sd)
    {
        sentences.Clear();
        
        foreach (var sentece in sd.senteces)
        {
            sentences.Enqueue(sentece);
        }

        if (!sd.isNarration)
            nameText.text = sd.name;
        else
            nameText.text = "";
        
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
                ResponsePlayer();
            }
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

    void ResponsePlayer()
    {
        dialogAnimator.SetBool("IsOpen", false);
        if (!hasResponse)
        {
            startConversationAnimator.SetBool("IsOpen", false);
            playerController.canMove = true;
            onResponse(-1);
            return;   
        }
        startConversationAnimator.SetBool("IsOpen", true);
    }

    public void OnClickResponse1()
    {
        onResponse(1);
        startConversationAnimator.SetBool("IsOpen", false);
        playerController.canMove = true;
    }
    
    public void OnClickResponse2()
    {
        onResponse(2);
        startConversationAnimator.SetBool("IsOpen", false);
        playerController.canMove = true;
    }

}
