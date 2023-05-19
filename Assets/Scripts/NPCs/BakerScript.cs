using System.Collections.Generic;
using UnityEngine;

public class BakerScript : MonoBehaviour
{
    public List<Item> toBuy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        GameManager.Instance.OpenShop(toBuy);
    }
}
