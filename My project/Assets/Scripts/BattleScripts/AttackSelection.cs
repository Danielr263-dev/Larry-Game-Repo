using UnityEngine;
using UnityEngine.UI;

public class AttackSelection : MonoBehaviour
{
    public Text attackButtonText; // The text inside the attack button
    public Button attackButton;
    public Button leftArrow;
    public Button rightArrow;
    public Text combatLogText;
    
    private bool isMelee = true; // Tracks whether Melee or Special is selected
    private int specialCooldown = 0; // Turns remaining until Special can be used

    void Start()
    {
        // Assign both buttons to toggle the attack type
        leftArrow.onClick.AddListener(ToggleAttack);
        rightArrow.onClick.AddListener(ToggleAttack);
        attackButton.onClick.AddListener(ConfirmAttack);

        UpdateAttackButton();
    }

    void ToggleAttack()
    {
        // Toggle between Melee and Special freely
        isMelee = !isMelee;

        UpdateAttackButton();
    }

    void UpdateAttackButton()
    {
        if (isMelee)
        {
            attackButtonText.text = "Melee";
            attackButton.interactable = true; // Always enable Melee
            attackButton.GetComponent<Image>().color = Color.white; // Normal color for Melee
        }
        else
        {
            if (specialCooldown > 0)
            {
                attackButtonText.text = "Special (Cooldown: " + specialCooldown + " turn" + (specialCooldown > 1 ? "s" : "") + ")";
                attackButton.interactable = false; // Disable Special when on cooldown
                attackButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f); // Gray out Special
            }
            else
            {
                attackButtonText.text = "Special";
                attackButton.interactable = true; // Enable Special when it's ready
                attackButton.GetComponent<Image>().color = Color.white; // Normal color for Special
            }
        }
    }

    void ConfirmAttack()
    {
        if (FindObjectOfType<AutoRPGSimulation>() != null)
        {
            FindObjectOfType<AutoRPGSimulation>().PlayerAttack(isMelee);

            // If the player used Special, start the cooldown
            if (!isMelee)
            {
                specialCooldown = 2; // Special attack is on cooldown for 2 turns
            }
        }
    }

    // This function is called from AutoRPGSimulation.cs after each turn
    public void ReduceCooldown()
    {
        if (specialCooldown > 0)
        {
            specialCooldown--;
        }
        UpdateAttackButton();
    }
}
