using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Transparency : MonoBehaviour
{
    [SerializeField] public Tilemap myMaterial;
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (col.name == "Player(Clone)")
        {
            
            Color color = myMaterial.color;
            color.a = 0.7f; 
            myMaterial.color = color; 
        }
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player(Clone)")
        {
            Color color = myMaterial.color;
            color.a = 255; 
            myMaterial.color = color; 
        }
    }
}
