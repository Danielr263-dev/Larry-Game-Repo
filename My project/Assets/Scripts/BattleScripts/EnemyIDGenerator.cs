using UnityEngine;

public class EnemyIDGenerator : MonoBehaviour
{
    public string enemyID; // Public variable to store the unique enemy ID.

    void Awake()
    {
        // Check if an ID already exists for this enemy.  We use PlayerPrefs and a key
        // that includes the GameObject's name to make it unique to this specific enemy.
        if (PlayerPrefs.HasKey("EnemyID_" + gameObject.name))
        {
            // Load the existing ID from PlayerPrefs.
            enemyID = PlayerPrefs.GetString("EnemyID_" + gameObject.name);
        }
        else
        {
            // Generate a new unique ID using System.Guid.NewGuid().  This creates a
            // very unlikely to be duplicated string.
            enemyID = System.Guid.NewGuid().ToString();

            // Save the new ID to PlayerPrefs.  The key is "EnemyID_" + the GameObject's name,
            // so each enemy's ID is stored separately.
            PlayerPrefs.SetString("EnemyID_" + gameObject.name, enemyID);
        }

        // Optional: Print the generated/loaded ID to the console for debugging.
        Debug.Log("Enemy ID for " + gameObject.name + ": " + enemyID);
    }
}