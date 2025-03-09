using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFill;  // Assign the UI Image component in the Inspector
    private int maxHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        healthFill.fillAmount = 1; // Start at full health
    }

    public void SetHealth(int health)
    {
        healthFill.fillAmount = (float)health / maxHealth;
    }
}
