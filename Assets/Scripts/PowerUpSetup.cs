using UnityEngine;

public class PowerUpSetup : MonoBehaviour
{
    [Header("Power-up Settings")]
    public GameObject powerUpPrefab;
    public float spawnInterval = 10f;
    public float spawnHeight = 5f;
    public float minX = -8f;
    public float maxX = 8f;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnPowerUp()
    {
        if (powerUpPrefab != null)
        {
            // Random X position within bounds
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

            // Spawn the power-up
            GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
            
            // Make sure it has the PowerUpItem component
            if (powerUp.GetComponent<PowerUpItem>() == null)
            {
                powerUp.AddComponent<PowerUpItem>();
            }
        }
    }
} 