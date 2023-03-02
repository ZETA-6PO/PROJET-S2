
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 200;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector3 velocity = new Vector3(0,0,0);
    
    void FixedUpdate()
    {
        float h_move = Input.GetAxis("Horizontal");
        float v_move = Input.GetAxis("Vertical");
        
        float horizontalMovement = h_move * moveSpeed * Time.deltaTime;
        float verticalMovement =  v_move * moveSpeed * Time.deltaTime;

        if (horizontalMovement != 0 && verticalMovement != 0)
        {
            double x = Math.Cos(90)*horizontalMovement;
            double y = Math.Sin(90)*verticalMovement;
            MovePlayer((float)x, (float)y);
        }
        else
        {
            MovePlayer(-horizontalMovement, verticalMovement);
        }
        animator.SetFloat("SpeedX", rb.velocity.x);
        animator.SetFloat("SpeedY", rb.velocity.y);
    }
    
    void MovePlayer(float _horizontalMovement, float _verticvalMovement)
    {
        Vector3 targetVelocity = new Vector2(-_horizontalMovement, _verticvalMovement);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .002f);
    }
}