using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using TMPro;
using UnityEngine.Events; // using text mesh for the clock display
 
using UnityEngine.Rendering; // used to access the volume component
 
public class DayNightScript : MonoBehaviour
{
    public Volume ppv; // this is the post processing volume
    
    public int mins;
    public int hours;

    public UnityEvent<int> onUpdateMinute;

    public bool activateLights; // checks if lights are on
    public GameObject[] lights; // all the lights we want on when its dark
    public SpriteRenderer[] stars; // star sprites 
    // Start is called before the first frame update
    public void Start()
    {
        //ppv = FindObjectOfType<Volume>();
    }
 
    // Update is called once per frame
    public void Update() // we used fixed update, since update is frame dependant. 
    {
        mins = GameManager.Instance.GetComponent<Time_Gestion>().mins;
        hours = GameManager.Instance.GetComponent<Time_Gestion>().hours;
        ControlPPV();
    }

    public void ControlPPV() // used to adjust the post processing slider.
    {
        //ppv.weight = 0;
        if(hours>=21 && hours<22) // dusk at 21:00 / 9pm    -   until 22:00 / 10pm
        {
            ppv.weight =  (float)mins / 60; // since dusk is 1 hr, we just divide the mins by 60 which will slowly increase from 0 - 1 
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, (float)mins / 60); // change the alpha value of the stars so they become visible
            }
 
            if (activateLights == false) // if lights havent been turned on
            {
                if (mins > 45) // wait until pretty dark
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(true); // turn them all on
                    }
                    activateLights = true;
                }
            }
        }

        if (hours >= 22 || hours < 6)
        {
            ppv.weight = 1;
            if (activateLights == false) // if lights havent been turned on
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].SetActive(true); // turn them all on
                }
                activateLights = true;
            }
        }
     
 
        if(hours>=6 && hours<7) // Dawn at 6:00 / 6am    -   until 7:00 / 7am
        {
            ppv.weight = 1 - (float)mins / 60; // we minus 1 because we want it to go from 1 - 0
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 1 -(float)mins / 60); // make stars invisible
            }
            if (activateLights == true) // if lights are on
            {
                if (mins > 45) // wait until pretty bright
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false); // shut them off
                    }
                    activateLights = false;
                }
            }
        }
    }

}