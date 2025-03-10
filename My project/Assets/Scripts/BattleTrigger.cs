using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTrigger : MonoBehaviour
{
    private Vector3 playerStartPosition;
    private string previousScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player touched the enemy
        {
            playerStartPosition = other.transform.position; // Save player's original position
            previousScene = SceneManager.GetActiveScene().name; // Store the current scene name

            PlayerPrefs.SetFloat("PlayerPosX", playerStartPosition.x); // Save X position
            PlayerPrefs.SetFloat("PlayerPosY", playerStartPosition.y); // Save Y position
            PlayerPrefs.SetString("PreviousScene", previousScene); // Save scene name

            SceneManager.LoadScene("FightScene"); // Load the battle scene
        }
    }
}
