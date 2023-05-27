using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SingleDialogue
{
    public string name;

    [TextArea(3,10)]
    public string[] senteces;

    public SingleDialogue(string name, string[] senteces)
    {
        if (name.Length == 0)
        {
            this.name = "Narrator";
        }
        else
        {
            this.name = name;   
        }
        this.senteces = senteces;
    }

    
}
