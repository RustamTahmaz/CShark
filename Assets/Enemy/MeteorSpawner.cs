using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public GameObject alienPrefab;
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    public float spawnHeightRangeMin = 3f;
    public float spawnHeightRangeMax = 5f;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void ScheduleNextSpawn()
    {
        float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke(nameof(SpawnRandomEnemy), interval);
    }

    void SpawnRandomEnemy()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        float cameraYPos = mainCamera.transform.position.y;

        float xPos = Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
        xPos = xPos > 0 ? cameraWidth / 2f + 1 : -cameraWidth / 2f - 1;
        float yPos = cameraYPos + Random.Range(spawnHeightRangeMin, spawnHeightRangeMax);
        Vector2 spawnPos = new Vector2(xPos, yPos - 10f);

        // Randomly choose which enemy to spawn
        GameObject prefabToSpawn = Random.value > 0.5f ? meteorPrefab : alienPrefab;
        GameObject clone = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // Optionally flip the sprite if spawning on the right
        if (xPos > 0)
        {
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.flipX = true;
        }

        ScheduleNextSpawn();
    }
}
