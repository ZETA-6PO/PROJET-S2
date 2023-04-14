using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This handle all the needed logic for the parents.
/// </summary>
public class parentsScript : MonoBehaviour
{
    [SerializeField] private GameObject momGO;
    
    [SerializeField] private GameObject dadGO;
    
    [SerializeField] private GameObject zoneGO;
    
    
    public UnityEvent onEnterZone;

    public UnityEvent onExitZone;

    public UnityEvent onTouchDad;

    public UnityEvent onTouchMom;

    public Vector3 momTransform;

    public Vector3 dadTransform;
    
    /// <summary>
    /// This function enable the parents object.
    /// </summary>
    public void Enable(bool dadEnabled, bool momEnabled, Vector3 MomTransform, Vector3 DadTransform, UnityAction onEnterZoneF, UnityAction onExitZoneF, UnityAction onTouchDadF, UnityAction onTouchMomF)
    {
        Debug.LogWarning("ITWORKS");
        momTransform = MomTransform;
        dadTransform = DadTransform;
        if (momEnabled)
        {
            momGO.SetActive(true);
            Debug.LogWarning("ITWORKS1");
            momGO.transform.position = momTransform;
        }

        if (dadEnabled)
        {
            dadGO.SetActive(true);
            Debug.LogWarning("ITWORKS2");
            dadGO.transform.position = dadTransform;
            
        }

        zoneGO.transform.localScale = new Vector3(Vector3.Distance(momTransform, dadTransform), Vector3.Distance(momTransform, dadTransform), 0)*1.2f;
        zoneGO.transform.position = (dadTransform+ momTransform) / 2;

        zoneGO.SetActive(true);
        
        onEnterZone.AddListener(onEnterZoneF);
        onExitZone.AddListener(onExitZoneF);
        onTouchDad.AddListener(onTouchDadF);
        onTouchMom.AddListener(onTouchMomF);
        
    }
    
    
    
    /// <summary>
    /// This function disable the parents object.
    /// </summary>
    public void Disable()
    {
        dadGO.SetActive(false);
        momGO.SetActive(false);
        zoneGO.SetActive(false);
        
        onEnterZone.RemoveAllListeners();
        onExitZone.RemoveAllListeners();
        onTouchDad.RemoveAllListeners();
        onTouchMom.RemoveAllListeners();
    }
    
}
