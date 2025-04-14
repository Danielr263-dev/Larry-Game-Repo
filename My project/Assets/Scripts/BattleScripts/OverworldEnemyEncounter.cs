using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemyEncounter : MonoBehaviour
{
    public string overworldEnemyID; // Public variable to store this enemy's unique ID.
    public string combatSceneName = "CombatScene"; // Public variable to store the name of your combat scene.
    public string playerTag = "Player"; // Public variable to store the tag of your player GameObject.
    public string enemyTag = "Enemy";  //Public variable to store the tag of enemy game object

    void Start()
    {
        // Get the unique ID from the EnemyIDGenerator script attached to this enemy.
        EnemyIDGenerator idGenerator = GetComponent<EnemyIDGenerator>();
        if (idGenerator != null)
        {
            overworldEnemyID = idGenerator.enemyID;
        }
        else
        {
            // Error handling:  If the EnemyIDGenerator is missing, log an error.
            Debug.LogError("EnemyIDGenerator not found on this GameObject!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)  //use trigger
    {
         // Check if the entering collider is the player OR an enemy.
        if (other.CompareTag(playerTag) || other.CompareTag(enemyTag))
        {
            // Store the enemy's unique ID in PlayerPrefs so the combat scene can access it.
            PlayerPrefs.SetString("CombatEnemyID", overworldEnemyID);
            // Store the name of the current scene so we can return to it after combat.
            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            // Load the combat scene.
            SceneManager.LoadScene(combatSceneName);
        }
    }

     void OnCollisionEnter2D(Collision2D collision)  //use collision
    {
         // Check if the entering collider is the player OR an enemy.
        if (collision.gameObject.CompareTag(playerTag) ||  collision.gameObject.CompareTag(enemyTag))
        {
            // Store the enemy's unique ID in PlayerPrefs so the combat scene can access it.
            PlayerPrefs.SetString("CombatEnemyID", overworldEnemyID);
            // Store the name of the current scene so we can return to it after combat.
            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            // Load the combat scene.
            SceneManager.LoadScene(combatSceneName);
        }
    }
}