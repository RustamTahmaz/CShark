using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnInterval = 2f;
    public float spawnHeightRangeMin = 3f;
    public float spawnHeightRangeMax = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMeteor), 1f, spawnInterval);
    }

    void SpawnMeteor()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // Get camera bounds in world space
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Get the camera's current Y position
        float cameraYPos = mainCamera.transform.position.y;
        

        // Random position: X off-screen, Y random between the given range
        float xPos = Random.Range(-cameraWidth / 2f, cameraWidth / 2f); // Random X within camera width
        xPos = xPos > 0 ? cameraWidth / 2f + 1 : -cameraWidth / 2f - 1; // Ensure spawn is off-screen to the left or right

        // Adjust Y spawn position relative to camera's Y position
        float yPos = cameraYPos + Random.Range(spawnHeightRangeMin, spawnHeightRangeMax); // Y position relative to camera

        // Create spawn position based on camera bounds
        Vector2 spawnPos = new Vector2(xPos, yPos - 10f);
        GameObject clone = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        // Optionally flip the meteor's sprite if it's spawning on the right side
        if (xPos > 0)
        {
            SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.flipX = true;
        }
    }
}
