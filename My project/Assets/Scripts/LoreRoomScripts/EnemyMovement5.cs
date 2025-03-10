using System.Collections;
using UnityEngine;

public class EnemyMovement5: MonoBehaviour
{
    public Transform pointA; // First waypoint (left)
    public Transform pointB; // Second waypoint (right)
    public float speed = 2f; // Movement speed
    public float pauseDuration = 1f; // Time to pause at each waypoint

    private Transform currentTarget; // Current target waypoint
    private bool isPaused = false; // Track if enemy is paused

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Waypoints (pointA or pointB) are not assigned in the Inspector!");
            return;
        }

        currentTarget = pointB; // Start by moving toward point B
    }

    void Update()
    {
        if (isPaused || currentTarget == null)
            return;

        // Move toward the current target
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Check if the enemy reached the waypoint
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            StartCoroutine(PauseAtWaypoint());
        }
    }

    IEnumerator PauseAtWaypoint()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);

        // Switch target between A and B
        currentTarget = (currentTarget == pointB) ? pointA : pointB;

        isPaused = false;
    }
}
