using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Attack")]
    public int baseAttackDamage = 10;

    [Header("Skills & Movement")]
    [Tooltip("How far (in world units) the player dashes vertically.")]
    public float dashDistance = 50f;

    [Tooltip("Rocket speed for initial launch, etc.")]
    public float rocketSpeed = 10f;

    // Optional event to notify listeners (UI, etc.) that health changed
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        // Initialize current health at start
        currentHealth = maxHealth;
         UpgradeMaxHealth(PlayerPrefs.GetInt("Upgrade_Health", 0), refillHealth: false);
        IncreaseAttack(PlayerPrefs.GetInt("Upgrade_Damage", 0));
        IncreaseDashDistance(PlayerPrefs.GetFloat("Upgrade_Dash", 0f));
        IncreaseRocketSpeed(PlayerPrefs.GetFloat("Upgrade_Rocket", 0f));
    }

    //--------------------- Health Methods ---------------------//

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void UpgradeMaxHealth(int amount, bool refillHealth = true)
    {
        maxHealth += amount;
        if (refillHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    //--------------------- Attack Methods ---------------------//

    public void IncreaseAttack(int amount)
    {
        baseAttackDamage += amount;
    }

    //--------------------- Movement Upgrades ---------------------//

    /// <summary>
    /// Increase how far the player can dash (e.g. from boots upgrade).
    /// </summary>
    public void IncreaseDashDistance(float amount)
    {
        dashDistance += amount;
    }

    /// <summary>
    /// Increase rocket speed (the initial launch velocity, for example).
    /// </summary>
    public void IncreaseRocketSpeed(float amount)
    {
        rocketSpeed += amount;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // e.g. disable movement, trigger a game over screen, etc.
    }
}
