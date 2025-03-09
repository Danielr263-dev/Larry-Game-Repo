using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector2 boundary = new Vector2(2f, 2f); // Boundary area (X and Y) where the camera starts following

    public float smoothSpeed = 0.2f; // Speed at which the camera follows the player
    public Vector3 offset = new Vector3(0, 10, -10); // Offset of the camera from the player

    // Hardcoded map bounds (calculated from offset and size)
    private Vector2 mapMinBounds = new Vector2(-8.90681f, -5.21200f); // Minimum bounds of the map
    private Vector2 mapMaxBounds = new Vector2(108.85709f, 5.11932f); // Maximum bounds of the map

    private Vector3 _cameraMinBoundary; // Minimum camera boundary in world space
    private Vector3 _cameraMaxBoundary; // Maximum camera boundary in world space
    private Vector3 _velocity = Vector3.zero; // Used for SmoothDamp

    void Start()
    {
        // Calculate the camera's boundaries based on its viewport
        CalculateCameraBoundaries();
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned!");
            return;
        }

        // Calculate the player's position relative to the camera's boundaries
        Vector3 playerPosition = player.position + offset;
        Vector3 desiredPosition = transform.position;

        // Check if the player is outside the camera's X boundary
        if (playerPosition.x < _cameraMinBoundary.x || playerPosition.x > _cameraMaxBoundary.x)
        {
            desiredPosition.x = playerPosition.x;
        }

        // Check if the player is outside the camera's Y boundary
        if (playerPosition.y < _cameraMinBoundary.y || playerPosition.y > _cameraMaxBoundary.y)
        {
            desiredPosition.y = playerPosition.y;
        }

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);

        // Clamp the camera's position to the map bounds
        smoothedPosition = ClampCameraToMap(smoothedPosition);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Recalculate the camera's boundaries after moving
        CalculateCameraBoundaries();
    }

    void CalculateCameraBoundaries()
    {
        // Convert the camera's viewport bounds to world space
        _cameraMinBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        _cameraMaxBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        // Adjust the boundaries based on the defined boundary area
        _cameraMinBoundary.x += boundary.x;
        _cameraMaxBoundary.x -= boundary.x;
        _cameraMinBoundary.y += boundary.y;
        _cameraMaxBoundary.y -= boundary.y;
    }

    Vector3 ClampCameraToMap(Vector3 position)
    {
        // Calculate the camera's half-width and half-height based on its viewport
        float cameraHalfHeight = Camera.main.orthographicSize; // For orthographic cameras
        float cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        // Clamp the camera's position to the map bounds
        float clampedX = Mathf.Clamp(position.x, mapMinBounds.x + cameraHalfWidth, mapMaxBounds.x - cameraHalfWidth);
        float clampedY = Mathf.Clamp(position.y, mapMinBounds.y + cameraHalfHeight, mapMaxBounds.y - cameraHalfHeight);

        return new Vector3(clampedX, clampedY, position.z);
    }
}

