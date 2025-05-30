using UnityEngine;

public class MeteorSpawner2 : MonoBehaviour
{
    [Header("Prefab & Timing")]
    public GameObject meteorPrefab;       // assign your Meteor prefab here
    public float      spawnInterval = 1f; // seconds between spawns

    [Header("Spawn Area")]
    public float xOffset    = 2f;   // how far offscreen to spawn
    public float spawnY     = -40f; // fixed Y coordinate

    [Header("Velocities")]
    public float horizontalSpeed = 2f;
    public float verticalSpeed   = 3f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        // Decide left or right
        int side = (Random.value < 0.5f) ? -1 : 1;
        float halfW = mainCam.orthographicSize * mainCam.aspect;
        float x = mainCam.transform.position.x + side * (halfW + xOffset);

        Vector3 spawnPos = new Vector3(x, spawnY, 0f);
        GameObject m = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // Set its velocity so it goes diagonally up toward center
        Rigidbody2D rb = m.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // if spawned on left (side=-1), -side=+1 → move right
            float vx = -side * horizontalSpeed;
            float vy = verticalSpeed;
            rb.linearVelocity = new Vector2(vx, vy);
        }
    }
}
