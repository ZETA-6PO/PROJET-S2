using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{

    private float timePassed = 0f;

    public Vector2 destination;

    public void Start()
    {
        destination = GameManager.Instance.displayedWaypoint;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 0.1f && GameManager.Instance.isWaypointActive)
        {
            timePassed = 0f;
            Vector2 direction = (destination - new Vector2(transform.position.x,transform.position.y)).normalized;
            transform.right = direction;
        }
        
    }
}
