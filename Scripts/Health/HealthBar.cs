using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarTotal;    // The full health bar image (background)
    public Image healthBarCurrent; // The current health bar image (foreground)

    private int maxHealth; // Total number of hearts (10 by default)

    public void SetMaxHealth(int health)
    {
        maxHealth = health;// Set the max health value
        SetHealth(health);         // Initialize the health bar
    }

    public void SetHealth(int health)
    {
        UpdateHealthBar(health);
    }

    private void UpdateHealthBar(int health)
    {
        // Calculate fill amount based on the fraction of current health to max health
        float fillAmount = (float)health / maxHealth;
        healthBarCurrent.fillAmount = fillAmount; // Update the fill amount of the health bar
    }
}
