
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 200;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector3 velocity;
    public bool canMove = true;
    public DirectionalArrow refDirectionalArrow;

    void FixedUpdate()
    {
        if (refDirectionalArrow.destination == Vector2.zero)
        {
            refDirectionalArrow.gameObject.SetActive(false);
        }
        else
        {
            refDirectionalArrow.gameObject.SetActive(true);
        }
        
        float h_move = canMove? Input.GetAxis("Horizontal") : 0;
        float v_move = canMove? Input.GetAxis("Vertical") : 0;
        
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

    public void teleportPlayerAt(Vector3 coord)
    {
        rb.transform.position = coord;
    }
}