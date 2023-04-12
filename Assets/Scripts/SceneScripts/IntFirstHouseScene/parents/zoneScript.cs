using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<parentsScript>().onEnterZone.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<parentsScript>().onExitZone.Invoke();
    }
}
