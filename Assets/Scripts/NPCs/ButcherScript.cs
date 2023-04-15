using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButcherScript : MonoBehaviour
{
    private UnityEvent onEnter;
    
    public void Enable(Vector3 butcherTransform, UnityEvent onSpeak)
    {
        transform.position = butcherTransform;
        gameObject.SetActive(true);

        onEnter = onSpeak;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        onEnter?.Invoke();
    }
}
