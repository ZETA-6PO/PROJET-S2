using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GaugeBar : MonoBehaviour
{
    private float v;
    private float maxValue;
    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Image img1;
    public Image img2;
    public Image img3;

    public void SetValue(float value)
    {
        v = value;
        BarUpdate(v,maxValue);
    }
    public void BarUpdate(float value,float mV)
    {
        v = value;
        maxValue = mV;
        float v1 = 0.3f * maxValue;
        float v2 = 2 * v1;
        if (v >= v2)
        {
            img1.sprite = sprite4;
            img2.sprite = sprite4;
            img3.sprite = ChooseSprite(v-v2);
        }
        else if (v >= v1)
        {
            img1.sprite = sprite4;
            img2.sprite = ChooseSprite(v-v1);
            img3.sprite = sprite0;
        }
        else
        {
            img1.sprite = ChooseSprite(v);
            img2.sprite = sprite0;
            img3.sprite = sprite0;
        }
    }
    

    private Sprite ChooseSprite(float v)
    {
        float v1 = 0.3f / 4 * maxValue;
        float v2 = 2 * v1;
        float v3 = 3 * v1;
        
        if (v==0)
        {
            return sprite0;
        }
        if (v<=v1)
        {
            return sprite1;
        }
        if (v<=v2)
        {
            return sprite2;
        }
        if (v<=v3)
        {
            return sprite3;
        }

        return sprite4;
    }
}