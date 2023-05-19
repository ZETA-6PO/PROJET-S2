using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This handle all the needed logic for the parents.
/// </summary>
public class GroupScript : MonoBehaviour
{

    
    
    public UnityEvent onEnterZone;

    public UnityEvent onExitZone;
    
    
    /// <summary>
    /// This function enable the parents object.
    /// </summary>
    public void Enable(UnityAction onEnterZoneF, UnityAction onExitZoneF)
    {
        onEnterZone.AddListener(onEnterZoneF);
        onExitZone.AddListener(onExitZoneF);

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        onEnterZone.Invoke();
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        onExitZone.Invoke();
    }
}
