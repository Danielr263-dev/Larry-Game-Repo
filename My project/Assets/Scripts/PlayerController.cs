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

        // Move player to the correct spawn point when entering a new scene
        SetSpawnPoint();
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

    string lastExit = SpawnManager.Instance.lastExitName;
    Debug.Log($"🔄 Looking for SpawnPoint matching: {lastExit}");

    GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

    if (spawnPoints.Length == 0)
    {
        Debug.LogError("🚨 No SpawnPoints found in the scene! Ensure you have GameObjects tagged as 'SpawnPoint'.");
        return;
    }

    Debug.Log($"✅ Found {spawnPoints.Length} SpawnPoints in the scene.");

    bool foundSpawn = false; // Flag to check if a valid spawn was found

    foreach (GameObject spawn in spawnPoints)
    {
        Debug.Log($"🔍 Checking SpawnPoint: {spawn.name} at Position: {spawn.transform.position}");

        if (spawn.name == lastExit)
        {
            Debug.Log($"✅ Player spawning at: {spawn.name} (Position: {spawn.transform.position})");

            // Move player to correct spawn point
            transform.position = spawn.transform.position;

            // Prevent immediate re-triggering (move slightly in opposite direction)
            Vector3 offset = Vector3.zero;

            if (lastExit.Contains("toWeapon"))      // Coming from Hallway to Weapon Room
                offset = new Vector3(0, -0.5f, 0);
            else if (lastExit.Contains("fromWeapon")) // Coming from Weapon Room to Hallway
                offset = new Vector3(0, 0.5f, 0);
            
            transform.position += offset;

            foundSpawn = true;
            break;
        }
    }

    if (!foundSpawn)
    {
        Debug.LogError($"❌ No matching SpawnPoint found for '{lastExit}'. Check if the name is correct.");
    }
}

    public bool IsMoving()
{
    return movement.sqrMagnitude > 0; // Checks if player is actually moving
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