using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEnemyEncounter : MonoBehaviour
{
    public string overworldEnemyID; // Unique enemy ID
    public string combatSceneName = "FightScene"; // Name of your battle scene
    public string playerTag = "Player"; 
    public string enemyTag = "Enemy";  

    void Start()
    {
        // Get unique ID from EnemyIDGenerator
        EnemyIDGenerator idGenerator = GetComponent<EnemyIDGenerator>();
        if (idGenerator != null)
        {
            overworldEnemyID = idGenerator.enemyID;

            // ✅ NEW: Check if this enemy was defeated already
            if (GameStateManager.instance != null && GameStateManager.instance.IsEnemyDefeated(overworldEnemyID))
            {
                gameObject.SetActive(false); // Disable this enemy if it was defeated
            }
        }
        else
        {
            Debug.LogError("EnemyIDGenerator not found on this GameObject!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) || other.CompareTag(enemyTag))
        {
            // Save enemy ID and scene name for battle scene
            PlayerPrefs.SetString("CombatEnemyID", overworldEnemyID);
            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(combatSceneName);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) || collision.gameObject.CompareTag(enemyTag))
        {
            PlayerPrefs.SetString("CombatEnemyID", overworldEnemyID);
            PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(combatSceneName);
        }
    }
}
