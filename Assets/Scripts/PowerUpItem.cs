using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [Header("Power-up Settings")]
    public float rotationSpeed = 100f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;
    public GameObject collectEffect;

    private Vector3 startPosition;
    private float bobPhase;

    private void Start()
    {
        startPosition = transform.position;
        bobPhase = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        // Rotate the power-up
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Bob up and down
        float newY = startPosition.y + Mathf.Sin((Time.time + bobPhase) * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply invincibility power-up
            PowerUpManager powerUpManager = FindFirstObjectByType<PowerUpManager>();
            if (powerUpManager != null)
            {
                powerUpManager.ActivateInvincibility();
            }

            // Add score
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScoreOnHit();
            }

            // Spawn collect effect if assigned
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            // Destroy the power-up
            Destroy(gameObject);
        }
    }
} 