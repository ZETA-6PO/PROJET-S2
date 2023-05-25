using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProducerScript : MonoBehaviour
{
    private UnityEvent onEnter;
    
    public void Enable(Vector3 studioMTransform, UnityAction onSpeak)
    {
        transform.position = studioMTransform;
        gameObject.SetActive(true);
        
        onEnter.AddListener(onSpeak);
    }
}
