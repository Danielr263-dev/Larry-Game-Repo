using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoRPGSimulation : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    public Text combatLogText;
    public GameObject attackSelectionUI; // Reference to Attack Selection UI Panel
    public AudioSource battleMusic; // Reference to AudioSource for battle music
    
    private int playerMaxHealth = 100;
    private int enemyMaxHealth = 80;

    private int playerCurrentHealth;
    private int enemyCurrentHealth;
    private bool battleOver = false;
    private bool waitingForPlayerInput = true; // Ensures player chooses an attack before continuing
    private AttackSelection attackSelection; // Reference to AttackSelection script

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;

        playerHealthBar.SetMaxHealth(playerMaxHealth);
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);

        attackSelection = FindObjectOfType<AttackSelection>(); // Get reference to AttackSelection

        attackSelectionUI.SetActive(true); // Ensure attack selection is visible at the start

        // Start battle music if assigned
        if (battleMusic != null)
        {
            battleMusic.loop = true; // Ensure looping
            battleMusic.Play();
        }

        StartCoroutine(StartBattleSequence());
    }

    IEnumerator StartBattleSequence()
    {
        UpdateCombatLog("Battle Starts!");
        yield return new WaitForSeconds(2f);
        UpdateCombatLog("Player is thinking...");
    }

    public void PlayerAttack(bool isMelee)
    {
        if (!waitingForPlayerInput || battleOver) return;

        waitingForPlayerInput = false;
        StartCoroutine(PlayerAttackSequence(isMelee));
    }

    IEnumerator SimulateTurn()
    {
        while (!battleOver)
        {
            // Wait for player attack selection
            waitingForPlayerInput = true;
            UpdateCombatLog("Player is thinking...");
            yield return new WaitUntil(() => !waitingForPlayerInput); // Wait for button press

            if (battleOver) yield break;

            // Enemy's Turn
            yield return StartCoroutine(EnemyAttackSequence());
        }
    }

    IEnumerator PlayerAttackSequence(bool isMelee)
    {
        if (battleOver) yield break;

        yield return new WaitForSeconds(1f);
        UpdateCombatLog($"Player uses {(isMelee ? "Melee" : "Special")} Attack!");
        yield return new WaitForSeconds(2f);

        int playerDamage = isMelee ? Random.Range(10, 20) : Random.Range(20, 30);
        enemyCurrentHealth -= playerDamage;
        enemyHealthBar.SetHealth(enemyCurrentHealth);
        yield return new WaitForSeconds(1.5f);

        if (enemyCurrentHealth <= 0)
        {
            EndBattle(true);
            yield break;
        }

        // Hide attack selection when the enemy is about to attack
        attackSelectionUI.SetActive(false);

        yield return StartCoroutine(EnemyAttackSequence());
    }

    IEnumerator EnemyAttackSequence()
    {
        if (battleOver) yield break;

        UpdateCombatLog("Enemy is thinking...");
        yield return new WaitForSeconds(2f);

        UpdateCombatLog("Enemy uses Basic Attack!");
        yield return new WaitForSeconds(2f);

        int enemyDamage = Random.Range(10, 15);
        playerCurrentHealth -= enemyDamage;
        playerHealthBar.SetHealth(playerCurrentHealth);
        yield return new WaitForSeconds(1.5f);

        if (playerCurrentHealth <= 0)
        {
            EndBattle(false);
        }
        else
        {
            // Reduce the cooldown after the enemy's turn
            attackSelection.ReduceCooldown();

            // Enable attack selection for the next player turn
            waitingForPlayerInput = true;
            UpdateCombatLog("Player is thinking...");
            
            // Show attack selection when it's the player's turn again
            attackSelectionUI.SetActive(true);
        }
    }

    void EndBattle(bool playerWon)
    {
        battleOver = true;

        if (playerWon)
        {
            UpdateCombatLog("Enemy defeated! You win!");
        }
        else
        {
            UpdateCombatLog("You have been defeated...");
        }

        // Stop the battle music when the battle ends
        if (battleMusic != null)
        {
            battleMusic.Stop();
        }

        // Hide UI at the end of battle
        attackSelectionUI.SetActive(false);
    }

    void UpdateCombatLog(string message)
    {
        combatLogText.text = message;
    }
}
