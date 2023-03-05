using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SingleDialogue
{
    public string name;
    public bool isNarration; //if its set to true then its a narration SingleDialogue
    
    [TextArea(3,10)]
    public string[] senteces;

    public SingleDialogue(string name, bool isNarration, string[] senteces)
    {
        this.name = name;
        this.isNarration = isNarration;
        this.senteces = senteces;
    }

    
}
