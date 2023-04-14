using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  This is the dadScript which handle the collision of the dad.
/// </summary>
public class dadScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        FindObjectOfType<parentsScript>().onTouchDad.Invoke();
    }
}
