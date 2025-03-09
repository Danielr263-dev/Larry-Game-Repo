using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoRPGSimulation : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    public Text combatLogText;

    private int playerMaxHealth = 100;
    private int enemyMaxHealth = 80;

    private int playerCurrentHealth;
    private int enemyCurrentHealth;
    private bool battleOver = false;

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        enemyCurrentHealth = enemyMaxHealth;

        playerHealthBar.SetMaxHealth(playerMaxHealth);
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);

        UpdateCombatLog("Simulation Started: Player vs Enemy.");
        
        // Start the Pokémon-style battle sequence
        StartCoroutine(SimulateTurn());
    }

    IEnumerator SimulateTurn()
    {
        while (!battleOver)
        {
            // Player's Turn
            yield return StartCoroutine(PlayerAttackSequence());

            if (battleOver) yield break;

            // Enemy's Turn
            yield return StartCoroutine(EnemyAttackSequence());
        }
    }

    IEnumerator PlayerAttackSequence()
    {
        if (battleOver) yield break;

        UpdateCombatLog("Player is thinking...");
        yield return new WaitForSeconds(2f);  // Slower transition

        int playerDamage = Random.Range(10, 20);
        UpdateCombatLog("Player used Melee Attack!");
        yield return new WaitForSeconds(2f);  // Longer delay before showing damage

        UpdateCombatLog($"Enemy takes {playerDamage} damage!");
        yield return new WaitForSeconds(2f);  // Longer delay before health bar updates

        enemyCurrentHealth -= playerDamage;
        enemyHealthBar.SetHealth(enemyCurrentHealth);
        yield return new WaitForSeconds(1.5f); // Pause after health change

        if (enemyCurrentHealth <= 0)
        {
            EndBattle(true);
            yield break;
        }
    }

    IEnumerator EnemyAttackSequence()
    {
        if (battleOver) yield break;

        UpdateCombatLog("Enemy is preparing an attack...");
        yield return new WaitForSeconds(2f);

        int enemyDamage = Random.Range(10, 15);
        UpdateCombatLog("Enemy used Basic Attack!");
        yield return new WaitForSeconds(2f);

        UpdateCombatLog($"Player takes {enemyDamage} damage!");
        yield return new WaitForSeconds(2f);

        playerCurrentHealth -= enemyDamage;
        playerHealthBar.SetHealth(playerCurrentHealth);
        yield return new WaitForSeconds(1.5f); // Pause after health change

        if (playerCurrentHealth <= 0)
        {
            EndBattle(false);
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
    }

    void UpdateCombatLog(string message)
    {
        combatLogText.text = message;
    }
}