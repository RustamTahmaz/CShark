using UnityEngine;

public class Laser : MonoBehaviour
{
    [Tooltip("World units per second")]
    public float speed = 20f;

    [Tooltip("Seconds before auto-destroy")]
    public float lifetime = 5f;

    void Start()
    {
        // ensures it goes away after the time limit
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // move “up” in local space (assuming your sprite points up)
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
                player.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
