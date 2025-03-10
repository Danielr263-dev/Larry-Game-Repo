using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;  // Name of the scene to transition to
    public string exitName;  // Name of the SpawnPoint in the next scene
    public AudioClip newMusic; // 🎵 Music for the next scene (null = keep current)
    
    private Collider2D transitionCollider;
    private bool canTransition = true; // Prevents immediate re-entry

    private void Start()
    {
        transitionCollider = GetComponent<Collider2D>();

        // Temporarily disable the trigger on scene load to prevent instant looping
        if (transitionCollider != null)
        {
            transitionCollider.enabled = false;
            Invoke(nameof(EnableTrigger), 0.5f); // Faster re-enable
        }

        // Prevent instant re-entry after spawn
        canTransition = false;
        StartCoroutine(EnableTransitionAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTransition)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            Debug.Log($"🚪 Player entered transition trigger for {sceneToLoad} from {gameObject.name}");

            // Ensure player is facing the door AND moving before transitioning
            if (player != null && IsFacingDoor(player) && player.IsMoving())
            {
                Debug.Log($"✅ Player is facing the door and moving, transitioning to {sceneToLoad}");

                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    canTransition = false; // Prevent re-triggering
                    SpawnManager.Instance.lastExitName = exitName; // Store the last exit

                    Debug.Log($"✅ Stored lastExitName: {SpawnManager.Instance.lastExitName}");

                    // 🎵 Handle music transition
                    HandleMusicTransition(sceneToLoad);

                    StartCoroutine(TransitionScene()); // Delayed transition to avoid issues
                }
                else
                {
                    Debug.LogError("❌ SceneTransition Error: sceneToLoad is empty! Assign a scene name in the Inspector.");
                }
            }
            else
            {
                Debug.Log("❌ Player is NOT facing the door or NOT moving, transition blocked!");
            }
        }
    }

    private void EnableTrigger()
    {
        if (transitionCollider != null)
        {
            transitionCollider.enabled = true;
        }
    }

    private IEnumerator EnableTransitionAfterDelay()
    {
        yield return new WaitForSeconds(0.3f); // Reduced cooldown for faster response
        canTransition = true;
    }

    private bool IsFacingDoor(PlayerController player)
    {
        Vector2 playerDirection = new Vector2(player.GetMoveX(), player.GetMoveY());

        // If player is standing still, assume they were facing the last direction
        if (playerDirection.sqrMagnitude == 0)
        {
            Debug.Log("🔄 Player was not moving, allowing transition.");
            return true;
        }

        // Check if the player is moving towards the door trigger
        Vector2 toDoor = ((Vector2)transform.position - (Vector2)player.transform.position).normalized;

        float dotProduct = Vector2.Dot(playerDirection.normalized, toDoor);
        Debug.Log($"📏 Dot Product: {dotProduct} (Higher means facing correctly)");

        // Loosen the angle restriction to make it less strict
        return dotProduct > 0.2f;
    }

    private IEnumerator TransitionScene()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to avoid accidental re-triggering
        SceneManager.LoadScene(sceneToLoad);
    }

    private void HandleMusicTransition(string nextScene)
    {
        if (MusicManager.Instance == null) return;

        if (nextScene == "BattleScene") 
        {
            MusicManager.Instance.StopMusic(); // ❌ Stop music in battle scenes
        }
        else if (newMusic != null) 
        {
            MusicManager.Instance.PlayMusic(newMusic, nextScene); // ✅ Change music if assigned
        }
    }
}
