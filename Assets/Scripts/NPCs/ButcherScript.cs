using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButcherScript : MonoBehaviour
{

    public List<Item> toBuy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Instance.OpenShop(toBuy,"Butchery");
    }
}
