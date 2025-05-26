using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [Header("Power-up Durations")]
    public float invincibilityDuration = 10f;
    public float speedBoostDuration = 15f;
    public float doublePointsDuration = 20f;

    [Header("Power-up Effects")]
    public float speedBoostMultiplier = 1.5f;
    public float pointsMultiplier = 2f;

    private bool isInvincible = false;
    private bool hasSpeedBoost = false;
    private bool hasDoublePoints = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateInvincibility()
    {
        if (!isInvincible)
        {
            StopCoroutine(nameof(InvincibilityCoroutine));
            StartCoroutine(nameof(InvincibilityCoroutine));
        }
    }

    public void ActivateSpeedBoost()
    {
        if (!hasSpeedBoost)
        {
            StopCoroutine(nameof(SpeedBoostCoroutine));
            StartCoroutine(nameof(SpeedBoostCoroutine));
        }
    }

    public void ActivateDoublePoints()
    {
        if (!hasDoublePoints)
        {
            StopCoroutine(nameof(DoublePointsCoroutine));
            StartCoroutine(nameof(DoublePointsCoroutine));
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        hasSpeedBoost = true;
        yield return new WaitForSeconds(speedBoostDuration);
        hasSpeedBoost = false;
    }

    private IEnumerator DoublePointsCoroutine()
    {
        hasDoublePoints = true;
        yield return new WaitForSeconds(doublePointsDuration);
        hasDoublePoints = false;
    }

    public bool IsInvincible() => isInvincible;
    public bool HasSpeedBoost() => hasSpeedBoost;
    public bool HasDoublePoints() => hasDoublePoints;

    public float GetSpeedMultiplier() => hasSpeedBoost ? speedBoostMultiplier : 1f;
    public float GetPointsMultiplier() => hasDoublePoints ? pointsMultiplier : 1f;
} 