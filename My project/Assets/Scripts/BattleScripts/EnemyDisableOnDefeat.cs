using UnityEngine;

public class EnemyDisableOnDefeat : MonoBehaviour
{
    private string enemyID; // Store the unique ID of this enemy.

    void Start()
    {
        // Get the enemy's ID from the EnemyIDGenerator script attached to this enemy.
        EnemyIDGenerator idGenerator = GetComponent<EnemyIDGenerator>();
        if (idGenerator != null)
        {
            enemyID = idGenerator.enemyID;
        }
        else
        {
            // Error handling: If the EnemyIDGenerator is missing, log an error.
            Debug.LogError("EnemyIDGenerator not found on this GameObject!");
        }

        // Check if the defeated enemy ID matches this enemy's ID.
        if (PlayerPrefs.HasKey("DefeatedEnemyID"))
        {
            string defeatedEnemyID = PlayerPrefs.GetString("DefeatedEnemyID");
            if (defeatedEnemyID == enemyID)
            {
                gameObject.SetActive(false); // Disable this enemy.
                PlayerPrefs.DeleteKey("DefeatedEnemyID"); // Clear the key after use.
            }
        }
    }
}