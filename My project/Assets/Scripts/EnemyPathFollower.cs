using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public PathData pathData; // Assign a PathData asset in the Inspector
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    private Vector3[] waypoints;
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (pathData != null)
        {
            waypoints = pathData.waypoints;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy.");
            enabled = false; // Disable the script if no waypoints are found
        }
    }

    void Update()
    {
        Vector3 currentWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (currentWaypoint - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Loop back to the first waypoint
            }
        }
    }
}