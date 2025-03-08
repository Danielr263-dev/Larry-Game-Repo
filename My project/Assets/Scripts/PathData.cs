using UnityEngine;

[CreateAssetMenu(fileName = "PathData", menuName = "ScriptableObjects/PathData", order = 1)]
public class PathData : ScriptableObject
{
    public Vector3[] waypoints; // Array of waypoint positions
}