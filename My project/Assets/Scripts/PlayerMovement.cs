using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of movement
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D
    }

    void Update()
    {
        // Get movement input
        movement.x = Input.GetAxisRaw("Horizontal"); // Left (-1) / Right (+1)
        movement.y = Input.GetAxisRaw("Vertical");   // Down (-1) / Up (+1)
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody2D
        rb.velocity = movement.normalized * moveSpeed;
    }
}
