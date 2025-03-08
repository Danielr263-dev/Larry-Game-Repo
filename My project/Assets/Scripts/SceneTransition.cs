using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;  // Name of the scene to transition to
    public string exitName;  // Name of the SpawnPoint in the next scene
    private Collider2D transitionCollider;
    private bool canTransition = true; // Prevents immediate re-entry

    private void Start()
    {
        transitionCollider = GetComponent<Collider2D>();

        // Temporarily disable the trigger on scene load to prevent instant looping
        if (transitionCollider != null)
        {
            transitionCollider.enabled = false;
            Invoke(nameof(EnableTrigger), 1.0f); // Re-enable after 1 second
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTransition)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null && IsFacingDoor(player))
            {
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    Debug.Log($"✅ Transitioning to {sceneToLoad} via {exitName}");

                    canTransition = false; // Prevent immediate re-trigger
                    transitionCollider.enabled = false; // Disable collider to avoid looping

                    SpawnManager.Instance.lastExitName = exitName; // Store the last exit
                    SceneManager.LoadScene(sceneToLoad);
                }
                else
                {
                    Debug.LogError("🚨 SceneTransition Error: sceneToLoad is empty! Assign a scene name in the Inspector.");
                }
            }
        }
    }

    private void EnableTrigger()
    {
        if (transitionCollider != null)
        {
            Debug.Log("🔄 Transition trigger re-enabled.");
            transitionCollider.enabled = true;
            canTransition = true; // Allow transitioning again
        }
    }

    private bool IsFacingDoor(PlayerController player)
    {
        Vector2 playerDirection = new Vector2(player.GetMoveX(), player.GetMoveY());

        // Check if the player is moving towards the door trigger
        Vector2 toDoor = (transform.position - player.transform.position).normalized;

        // Only transition if player is facing towards the door
        return Vector2.Dot(playerDirection, toDoor) > 0.5f;
    }
}
