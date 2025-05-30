using UnityEngine;
using System.Collections;

public class UFOController : MonoBehaviour
{
    [Tooltip("Horizontal speed (world units/sec)")]
    public float moveSpeed = 3f;

    [Tooltip("Distance on X‐axis to player before stopping")]
    public float stopThreshold = 0.2f;

    private Transform player;
    private int      moveDir = -1;    // set by the spawner: -1 = leftward, +1 = rightward
    private bool     hasStopped = false;
    private UFOLaserShooter shooter;

    void Awake()
    {
        player  = GameObject.FindWithTag("Player")?.transform;
        shooter = GetComponent<UFOLaserShooter>();

        // Disable automatic firing; we’ll trigger it when aligned
        if (shooter != null)
            shooter.enabled = false;
    }

    void Update()
    {
        if (!hasStopped)
        {
            // Move horizontally
            transform.position += Vector3.right * (moveDir * moveSpeed * Time.deltaTime);

            // Check alignment to player's X
            if (player != null &&
                Mathf.Abs(transform.position.x - player.position.x) <= stopThreshold)
            {
                hasStopped = true;
                if (shooter != null)
                    shooter.enabled = true;        // turn on shooting
            }
        }
    }

    /// <summary>
    /// Called by the spawner to tell the UFO which direction to move.
    /// </summary>
    public void SetMoveDirection(int direction)
    {
        moveDir = direction > 0 ? 1 : -1;
        // flip sprite if needed:
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * moveDir;
        transform.localScale = s;
    }
}
