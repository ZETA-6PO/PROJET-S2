using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Time_Gestion : MonoBehaviour
{
    
    public float tick; // Increasing the tick, increases second rate
    public float seconds; 
    public int mins;
    public int hours;
    
    public UnityEvent<int> onUpdateMinute;
    
    
    public void Update() // we used fixed update, since update is frame dependant. 
    {
        CalcTime();
    }
 
    public void CalcTime() // Used to calculate sec, min and hours
    {
        seconds += Time.fixedDeltaTime * tick; // multiply time between fixed update by tick
 
        if (seconds >= 60) // 60 sec = 1 min
        {
            seconds = 0;
            mins += 1;
        }
 
        if (mins >= 60) //60 min = 1 hr
        {
            mins = 0;
            hours += 1;
            onUpdateMinute.Invoke(hours);
        }
 
        if (hours >= 24) //24 hr = 1 day
        {
            hours = 0;
        }
    }
}
