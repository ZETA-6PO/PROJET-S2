using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Animator MiniMapAnimator;
    

    public void EnableMiniMap()
    {
        MiniMapAnimator.SetBool("IsOpen", true);
    }

    public void DisableMiniMap()
    {
        MiniMapAnimator.SetBool("IsOpen", false);
    }
    
}