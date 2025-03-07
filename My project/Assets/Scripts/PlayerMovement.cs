using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get player input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize movement to avoid diagonal speed boost
        movement = movement.normalized;

        // Set Animator parameters
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("isMoving", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.velocity = movement * moveSpeed;
    }
}
