using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Luv_Restla_script : MonoBehaviour
{
    private UnityEvent onEnter;
    
    public void Enable(Vector3 LuvTransform, UnityEvent onSpeak)
    {
        transform.position = LuvTransform;
        gameObject.SetActive(true);

        onEnter = onSpeak;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        onEnter?.Invoke();
    }
}
