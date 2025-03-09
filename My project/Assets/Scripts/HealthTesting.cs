using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealthUI : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;
    private int playerHealth = 100;
    private int enemyHealth = 100;

    void Start()
    {
        playerHealthBar.SetMaxHealth(100);
        enemyHealthBar.SetMaxHealth(100);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Player takes damage
        {
            playerHealth -= 10;
            playerHealthBar.SetHealth(playerHealth);
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Enemy takes damage
        {
            enemyHealth -= 10;
            enemyHealthBar.SetHealth(enemyHealth);
        }
    }
}

