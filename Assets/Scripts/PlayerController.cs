
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 500;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector3 velocity = new Vector3(0,0,0);
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement, verticalMovement);
        animator.SetFloat("SpeedX", rb.velocity.x);
        animator.SetFloat("SpeedY", rb.velocity.y);
    }
    
    void MovePlayer(float _horizontalMovement, float _verticvalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, _verticvalMovement);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
    }
}