using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutoRPGSimulation : MonoBehaviour
{
    // Public variables for linking UI elements and GameObjects in the Unity Editor.
    public HealthBar playerHealthBar; // Health bar for the player.
    public HealthBar enemyHealthBar; // Health bar for the enemy.
    public Text combatLogText; // Text element to display combat messages.
    public GameObject attackSelectionUI; // UI panel for player attack selection.
    public AudioSource battleMusic; // Audio source for battle music.
    public GameObject battleEnemy; // GameObject representing the enemy in battle.
    public GameObject battlePlayer; // GameObject representing the player in battle.
    public float specialAttackChance = 0.3f; // Chance for the enemy to use a special attack.

    // Public string variables to store the names of the player and enemy GameObjects.
    public string playerObjectName = "battlePlayer"; // Name of the player GameObject.
    public string enemyObjectName = "battleEnemy"; // Name of the enemy GameObject.

    // Private variables to manage game state and data.
    private int playerMaxHealth = 100; // Maximum health of the player.
    private int enemyMaxHealth = 80; // Maximum health of the enemy.
    private int playerCurrentHealth; // Current health of the player.
    private int enemyCurrentHealth; // Current health of the enemy.
    private bool battleOver = false; // Flag to indicate if the battle is over.
    private bool waitingForPlayerInput = true; // Flag to check if the player needs to choose an attack.
    private AttackSelection attackSelection; // Reference to the AttackSelection script.
    private EnemyAnimatorController enemyAnimator; // Reference to the enemy's animator controller.
    private PlayerAnimatorController playerAnimator; // Reference to the player's animator controller.

    void Start()
    {
        // Initialize player and enemy health.
        playerCurrentHealth = playerMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;

        // Set the maximum health values for the health bars.
        playerHealthBar.SetMaxHealth(playerMaxHealth);
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);

        // Find and enable the attack selection UI.
        attackSelection = FindObjectOfType<AttackSelection>();
        attackSelectionUI.SetActive(true);

        // Play the battle music if it's assigned.
        if (battleMusic != null)
        {
            battleMusic.loop = true;
            battleMusic.Play();
        }

        // Find the player and enemy GameObjects by their names.
        battlePlayer = GameObject.Find(playerObjectName);
        battleEnemy = GameObject.Find(enemyObjectName);

        // Debugging: Verify that the player and enemy GameObjects were found.
        if (battlePlayer == null) Debug.LogError("Player object not found with name: " + playerObjectName);
        if (battleEnemy == null) Debug.LogError("Enemy object not found with name: " + enemyObjectName);

        // Get the animator controllers for the enemy and player.
        if (battleEnemy != null)
        {
            enemyAnimator = battleEnemy.GetComponent<EnemyAnimatorController>();
        }

        if (battlePlayer != null)
        {
            playerAnimator = battlePlayer.GetComponent<PlayerAnimatorController>();
        }

        // Start the battle sequence.
        StartCoroutine(StartBattleSequence());
    }

    // Coroutine to start the battle sequence.
    IEnumerator StartBattleSequence()
    {
        UpdateCombatLog("Battle Starts!");
        yield return new WaitForSeconds(2f);
        UpdateCombatLog("Player is thinking...");
    }

    // Public method called when the player chooses an attack.
    public void PlayerAttack(bool isMelee)
    {
        if (!waitingForPlayerInput || battleOver) return; // Exit if not waiting for input or battle is over.

        waitingForPlayerInput = false; // Set waiting flag to false.
        StartCoroutine(PlayerAttackSequence(isMelee)); // Start the player's attack sequence.
    }

    // Coroutine to simulate a turn of the battle.
    IEnumerator SimulateTurn()
    {
        while (!battleOver) // Loop until the battle is over.
        {
            waitingForPlayerInput = true; // Set waiting flag to true.
            UpdateCombatLog("Player is thinking...");
            yield return new WaitUntil(() => !waitingForPlayerInput); // Wait for player input.

            if (battleOver) yield break; // Exit if the battle is over.

            yield return StartCoroutine(EnemyAttackSequence()); // Start the enemy's attack sequence.
        }
    }

    // Coroutine for the player's attack sequence.
    IEnumerator PlayerAttackSequence(bool isMelee)
    {
        if (battleOver) yield break; // Exit if the battle is over.

        yield return new WaitForSeconds(1f);
        UpdateCombatLog($"Player uses {(isMelee ? "Basic" : "Special")} Attack!");

        // Trigger the player's attack animation.
        if (playerAnimator != null)
        {
            playerAnimator.PerformAction(isMelee ? PlayerAnimatorController.PlayerAction.basicAttack : PlayerAnimatorController.PlayerAction.specialAttack);
        }

        yield return new WaitForSeconds(2f);

        // Calculate and apply damage to the enemy.
        int playerDamage = isMelee ? Random.Range(10, 20) : Random.Range(20, 30);
        enemyCurrentHealth -= playerDamage;
        enemyHealthBar.SetHealth(enemyCurrentHealth);
        yield return new WaitForSeconds(1.5f);

        // Check if the enemy is defeated.
        if (enemyCurrentHealth <= 0)
        {
            if (enemyAnimator != null)
                enemyAnimator.PerformAction(EnemyAnimatorController.EnemyAction.death);

            EndBattle(true); // End the battle with player win.
            yield break;
        }

        attackSelectionUI.SetActive(false); // Hide attack selection UI.
        yield return StartCoroutine(EnemyAttackSequence()); // Start the enemy's attack sequence.
    }

    // Coroutine for the enemy's attack sequence.
    IEnumerator EnemyAttackSequence()
    {
        if (battleOver) yield break; // Exit if the battle is over.

        UpdateCombatLog("Enemy is thinking...");
        yield return new WaitForSeconds(2f);

        // Determine if the enemy uses a special attack.
        bool useSpecial = Random.value < specialAttackChance;

        // Trigger the enemy's attack animation and log the attack type.
        if (useSpecial)
        {
            UpdateCombatLog("Enemy uses Special Attack!");
            if (enemyAnimator != null)
                enemyAnimator.PerformAction(EnemyAnimatorController.EnemyAction.specialAttack);
        }
        else
        {
            UpdateCombatLog("Enemy uses Basic Attack!");
            if (enemyAnimator != null)
                enemyAnimator.PerformAction(EnemyAnimatorController.EnemyAction.basicAttack);
        }

        yield return new WaitForSeconds(2f);

        // Calculate and apply damage to the player.
        int enemyDamage = useSpecial ? Random.Range(20, 30) : Random.Range(10, 15);
        playerCurrentHealth -= enemyDamage;
        playerHealthBar.SetHealth(playerCurrentHealth);
        yield return new WaitForSeconds(1.5f);

        // Check if the player is defeated.
        if (playerCurrentHealth <= 0)
        {
            EndBattle(false); // End the battle with enemy win.
        }
        else
        {
            attackSelection.ReduceCooldown(); // Reduce the cooldown of the attack selection.
            waitingForPlayerInput = true; // Set waiting flag to true.
            UpdateCombatLog("Player is thinking...");
            attackSelectionUI.SetActive(true); // Show attack selection UI.
        }
    }

    // Method to end the battle.
    void EndBattle(bool playerWon)
    {
        battleOver = true; // Set the battle over flag to true.

        // Log the battle result.
        if (playerWon)
        {
            UpdateCombatLog("Enemy defeated! You win!");
        }
        else
        {
            UpdateCombatLog("You have been defeated...");
        }

        // Stop the battle music.
        if (battleMusic != null)
        {
            battleMusic.Stop();
        }

        attackSelectionUI.SetActive(false); // Hide attack selection UI.
        StartCoroutine(ReturnToPreviousScene()); // Start the return to previous scene sequence.
    }

    // Coroutine to return to the previous scene.
    IEnumerator ReturnToPreviousScene()
    {
        yield return new WaitForSeconds(3f); // Wait for a short delay.

        // Load the previous scene and player position from PlayerPrefs.
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainHallway");
        float playerX = PlayerPrefs.GetFloat("PlayerPosX", 0);
        float playerY = PlayerPrefs.GetFloat("PlayerPosY", 0);

        // Store the player's respawn position.
        PlayerPrefs.SetFloat("RespawnX", playerX);
        PlayerPrefs.SetFloat("RespawnY", playerY);

        SceneManager.LoadScene(previousScene); // Load the previous scene.
    }

    // Method to update the combat log text.
    void UpdateCombatLog(string message)
    {
        combatLogText.text = message;
    }
}