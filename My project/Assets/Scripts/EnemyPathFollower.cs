using UnityEngine;
using System.Collections;

public class EnemyPathFollower : MonoBehaviour
{
    public PathData pathData; // Assign a PathData asset in the Inspector
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float pauseDuration = 2f; // Time to pause at each waypoint

    private Vector3[] waypoints;
    private int currentWaypointIndex = 0; // Start at the first waypoint (index 0)
    private bool isMovingUp = true; // Determines movement direction
    private bool isWaiting = false; // Prevents movement during pause

    void Start()
    {
        if (pathData == null)
        {
            Debug.LogError("PathData is not assigned in the Inspector.");
            enabled = false;
            return;
        }

        waypoints = pathData.waypoints;

        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("At least two waypoints are required.");
            enabled = false;
            return;
        }

        Debug.Log("EnemyPathFollower initialized with " + waypoints.Length + " waypoints.");
        transform.position = waypoints[0]; // Ensure the enemy starts at waypoint 0
        StartCoroutine(MoveToNextWaypoint()); // Begin movement coroutine
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            // Check if we are in a waiting state, if so, stop any movement
            if (isWaiting)
            {
                yield return null; // Skip this frame, ensure no movement during the pause
                continue; // Skip the rest of the loop, don't process movement
            }

            Vector3 targetWaypoint = waypoints[currentWaypointIndex];

            // Move towards the waypoint if not waiting
            while (Vector3.Distance(transform.position, targetWaypoint) > 0.1f)
            {
                if (isWaiting)
                    break; // Exit movement loop if we're waiting

                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, moveSpeed * Time.deltaTime);

                // Rotate towards the target waypoint
                Vector3 direction = (targetWaypoint - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    angle -= 90; // Adjust for vertical orientation
                    Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                yield return null; // Wait until next frame
            }

            // Now that we're close to the waypoint, pause before continuing
            yield return StartCoroutine(PauseAtWaypoint());

            // Toggle movement direction after pausing
            if (isMovingUp)
                currentWaypointIndex = 1; // Move to waypoint 1
            else
                currentWaypointIndex = 0; // Move to waypoint 0

            isMovingUp = !isMovingUp;
        }
    }

    IEnumerator PauseAtWaypoint()
    {
        isWaiting = true; // Stop movement temporarily
        Debug.Log("Reached waypoint " + currentWaypointIndex + ". Pausing...");

        // Use WaitForSeconds to handle the pause duration
        yield return new WaitForSeconds(pauseDuration);

        isWaiting = false; // Resume movement
        Debug.Log("Pause finished at waypoint " + currentWaypointIndex); // Log when pause finishes
    }
}
