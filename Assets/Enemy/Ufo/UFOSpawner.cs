using UnityEngine;

public class UFOSpawner : MonoBehaviour
{
    [Tooltip("Your UFO prefab (must have UFOController + UFOLaserShooter)")]
    public GameObject ufoPrefab;

    [Tooltip("Seconds between spawns")]
    public float spawnInterval = 3f;

    [Tooltip("How far off‐screen to spawn (in world units)")]
    public float xOffset = 2f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        InvokeRepeating(nameof(SpawnUFO), 1f, spawnInterval);
    }

    void SpawnUFO()
    {
        // pick left or right
        int side = Random.value < 0.5f ? -1 : 1;
        float camHalfW = cam.orthographicSize * cam.aspect;
        float x = cam.transform.position.x + side * (camHalfW + xOffset);

        // ─── NEW: force Y between –50 and –40 ─────────
        float y = Random.Range(-50f, -40f);
        // ───────────────────────────────────────────────

        GameObject ufo = Instantiate(ufoPrefab, new Vector3(x, y, 0), Quaternion.identity);

        var ctrl = ufo.GetComponent<UFOController>();
        if (ctrl != null)
            ctrl.SetMoveDirection(-side);
    }
}
