using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BakerScript : MonoBehaviour
{
    private UnityEvent onEnter;
    
    public void Enable(Vector3 bakerTransform, UnityEvent onSpeak)
    {
        transform.position = bakerTransform;
        gameObject.SetActive(true);

        onEnter = onSpeak;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        onEnter?.Invoke();
    }
}
