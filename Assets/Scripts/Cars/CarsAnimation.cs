using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsAnimation : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;

    private Transform target;
    private int destPoint = 0;

    public Animator animator;
    private Vector2 mouvement;

    private bool is_Waiting;
    
    void Start()
    {
        target = waypoints[0];
    }
    
    IEnumerator tempWait()
    {
        speed = 0;
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("Rotation", 0);
        speed = 5;
        is_Waiting = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.3f && !is_Waiting)
        {
            Transform precedent = new RectTransform(); 
            if (destPoint == 0)
            {
                precedent = waypoints[waypoints.Length - 1];
            }
            else
            {
                precedent = waypoints[destPoint - 1];
            }

            Transform act = target;
            
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];

            if (precedent.position.x > target.position.x)
            {
                if (precedent.position.y < target.position.y)
                {
                    if (precedent.position.x > act.position.x)
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 1); //Right ->  UP
                        StartCoroutine(tempWait());
                    }
                    else
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 2); //Down -> Left   
                        StartCoroutine(tempWait());
                    }
                }

                if (precedent.position.y > target.position.y)
                {
                    if (precedent.position.x > act.position.x)
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 3); // Right -> Down
                        StartCoroutine(tempWait());
                    }
                    else
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 4); // Up - > Left
                        StartCoroutine(tempWait());
                    }
                }
                // else : Right-Left Pas d'Anim
            }

            if (precedent.position.x < target.position.x)
            {
                if (precedent.position.y < target.position.y)
                {
                    if (precedent.position.x < act.position.x)
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 5); // Left -> Up
                        StartCoroutine(tempWait());
                    }
                    else
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 6); // Down -> Right
                        StartCoroutine(tempWait());
                    }
                }

                if (precedent.position.y > target.position.y)
                {
                    if (precedent.position.x < act.position.x)
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 7); // Left -> Down
                        StartCoroutine(tempWait());
                    }
                    else
                    {
                        is_Waiting = true;
                        animator.SetInteger("Rotation", 8); // Up -> Right
                        StartCoroutine(tempWait());
                    }
                }
                // else : Left -> Right
            }
            // else : precedent.x = target.x Up -> Down // Down -> Up

        }

        mouvement.x = Vector3.Normalize(dir).x;
        mouvement.y = Vector3.Normalize(dir).y;
        if (!is_Waiting)
        {
            animator.SetFloat("Horizontal", mouvement.x);
            animator.SetFloat("Vertical", mouvement.y);   
        }
    }
}
