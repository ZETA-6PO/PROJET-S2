using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCpatrol : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;

    private Transform target;
    private int destPoint = 0;

    public Animator animator;
    private Vector2 mouvement;
    public Rigidbody2D rb;
    
    void Start()
    {
        target = waypoints[0];
        rb = GetComponent<Rigidbody2D>();
        speed = 400f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        rb.velocity = dir.normalized * speed * Time.deltaTime;
        //transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];
        }

        mouvement.x = Vector3.Normalize(dir).x;
        mouvement.y = Vector3.Normalize(dir).y;
        
        animator.SetFloat("Horizontal", mouvement.x);
        animator.SetFloat("Vertical", mouvement.y);
    }
}
