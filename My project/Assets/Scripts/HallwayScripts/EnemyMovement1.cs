using UnityEngine;
using System.Collections;

public class EnemyMovement1 : MonoBehaviour
{
    public GameObject pointA; // First waypoint
    public GameObject pointB; // Second waypoint
    private Rigidbody2D rb;   // Rigidbody2D component
    private Transform CurrentPoint; // Current target waypoint

    public float speed = 2f; // Movement speed
    public float pauseDuration = 1f; // Time to pause at each waypoint

    private bool isPaused = false; // Track if the enemy is currently paused

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentPoint = pointB.transform; // Start by moving toward pointB
    }

    void Update()
    {
        // If the enemy is paused, do nothing
        if (isPaused)
        {
            return;
        }

        // Move toward the CurrentPoint
        if (CurrentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(0, speed); // Move upward (toward pointB)
        }
        else
        {
            rb.velocity = new Vector2(0, -speed); // Move downward (toward pointA)
        }

        // Check if the enemy is close to the CurrentPoint
        if (Vector2.Distance(transform.position, CurrentPoint.position) < 0.2f)
        {
            // Stop moving and start the pause coroutine
            rb.velocity = Vector2.zero; // Stop the enemy
            StartCoroutine(PauseAtWaypoint());
        }
    }

    IEnumerator PauseAtWaypoint()
    {
        // Set the enemy to paused state
        isPaused = true;

        // Wait for the pause duration
        yield return new WaitForSeconds(pauseDuration);

        // Switch the target point
        if (CurrentPoint == pointB.transform)
        {
            CurrentPoint = pointA.transform; // Switch to pointA
        }
        else
        {
            CurrentPoint = pointB.transform; // Switch to pointB
        }

        // Resume movement
        isPaused = false;
    }
}