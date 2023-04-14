using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Bar",menuName ="ATH/GaugeBar")]
public class GaugeBar : ScriptableObject
{
    public int value;
    public int maxValue;
    public Sprite[] sprites;
}

public class BarController : MonoBehaviour
{
    public GaugeBar bar;
    public Sprite slot1;
    public Sprite slot2;
    public Sprite slot3;

    public void Add(int count)
    {
        bar.value += count;
    }

    private void Update()
    {
        
    }
}