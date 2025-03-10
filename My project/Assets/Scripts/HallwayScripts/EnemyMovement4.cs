using UnityEngine;
using System.Collections;

public class EnemyMovement4 : MonoBehaviour
{
    public GameObject pointA; // First waypoint
    public GameObject pointB; // Second waypoint
    public GameObject pointC; // Third waypoint
    public GameObject pointD; // Fourth waypoint
    private Rigidbody2D rb;   // Rigidbody2D component

    public float speed = 2f; // Movement speed
    public float pauseDuration = 1f; // Time to pause at each waypoint

    private Transform[] waypoints; // Stores waypoints A, B, C, D
    private int currentWaypointIndex = 0; // Tracks the current waypoint
    private int direction = 1; // 1 = moving forward, -1 = moving backward
    private bool isPaused = false; // Track if the enemy is currently paused

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Define waypoints order
        waypoints = new Transform[] { pointA.transform, pointB.transform, pointC.transform, pointD.transform };

        // Start at point A
        currentWaypointIndex = 0;
    }

    void Update()
    {
        // If the enemy is paused, do nothing
        if (isPaused)
            return;

        // Move toward the current waypoint
        Vector2 directionToMove = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        rb.velocity = directionToMove * speed;

        // Check if the enemy is close to the target waypoint
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.2f)
        {
            rb.velocity = Vector2.zero; // Stop the enemy
            StartCoroutine(PauseAtWaypoint());
        }
    }

    IEnumerator PauseAtWaypoint()
    {
        isPaused = true; // Stop movement

        yield return new WaitForSeconds(pauseDuration); // Wait at waypoint

        // Move forward or backward based on direction
        currentWaypointIndex += direction;

        // Reverse direction if we reach either end (A or D)
        if (currentWaypointIndex == waypoints.Length - 1 || currentWaypointIndex == 0)
        {
            direction *= -1; // Reverse movement direction
        }

        isPaused = false; // Resume movement
    }
}
