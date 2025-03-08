using UnityEngine;
using System.Collections; // Required for coroutine

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

        // Move player to the correct spawn point when entering a new scene
        SetSpawnPoint();

        // Temporarily disable the player's collider to prevent instant re-triggering
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            StartCoroutine(DisableColliderTemporarily(playerCollider));
        }
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

    private void SetSpawnPoint()
    {
        // Check if SpawnManager exists to get the last exit name
        if (SpawnManager.Instance == null)
        {
            Debug.LogError("🚨 SpawnManager.Instance is NULL! Make sure it is initialized.");
            return;
        }

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("🚨 No SpawnPoints found in the scene! Ensure you have GameObjects tagged as 'SpawnPoint'.");
            return;
        }

        Debug.Log("✅ Found " + spawnPoints.Length + " SpawnPoints in the scene.");

        foreach (GameObject spawn in spawnPoints)
        {
            Debug.Log("🔍 Checking SpawnPoint: " + spawn.name);
            
            if (spawn.name == SpawnManager.Instance.lastExitName)
            {
                Debug.Log("✅ Player spawning at: " + spawn.name);
                transform.position = spawn.transform.position; // Move player to correct spawn point

                // Move player slightly forward to prevent immediate re-triggering
                transform.position += new Vector3(0, 0.5f, 0);

                break;
            }
        }
    }

    IEnumerator DisableColliderTemporarily(Collider2D collider)
    {
        Debug.Log("⏳ Temporarily disabling player collider to prevent re-trigger...");
        collider.enabled = false;
        yield return new WaitForSeconds(1.0f); // Adjust timing if needed
        collider.enabled = true;
        Debug.Log("✅ Player collider re-enabled.");
    }

    public float GetMoveX()
    {
        return movement.x;
    }

    public float GetMoveY()
    {
        return movement.y;
    }
}
