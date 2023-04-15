using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicShopSalerScript : MonoBehaviour
{
    public class BakerScript : MonoBehaviour
    {
        private UnityEvent onEnter;
    
        public void Enable(Vector3 musicSSTransform, UnityEvent onSpeak)
        {
            transform.position = musicSSTransform;
            gameObject.SetActive(true);

            onEnter = onSpeak;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            onEnter?.Invoke();
        }
    }
}
