using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StudioManagerScript : MonoBehaviour
{
    private UnityEvent onEnter;
    
    public void Enable(Vector3 studioMTransform, UnityEvent onSpeak)
    {
        transform.position = studioMTransform;
        gameObject.SetActive(true);

        onEnter = onSpeak;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        onEnter?.Invoke();
    }
}
