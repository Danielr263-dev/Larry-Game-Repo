using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.5f;  // Walking speed
    public float runSpeed = 5.0f;   // Running speed (Shift key)
    
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Create movement vector
        movement = new Vector2(moveX, moveY);

        // Normalize movement to prevent diagonal speed boost
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }

        // Check if player is moving
        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
        }
        else
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }
    }

    void FixedUpdate()
    {
        // Stop movement instantly when not moving
        if (movement.sqrMagnitude == 0)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Choose speed based on running or walking
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // Apply movement speed
            rb.velocity = movement * speed;
        }
    }
}
