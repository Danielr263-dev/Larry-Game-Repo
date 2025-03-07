using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variable to control movement speed
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow keys
        float moveY = 0f;

        // Use Up/Down Arrow keys for vertical movement
        if (Input.GetKey(KeyCode.UpArrow))
            moveY = 1f; // Move up
        else if (Input.GetKey(KeyCode.DownArrow))
            moveY = -1f; // Move down

        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow keys

        // Create a movement vector based on input
        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        // Normalize the vector to prevent faster diagonal movement
        if (movement.magnitude > 1f)
            movement = movement.normalized;

        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}