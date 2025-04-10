using UnityEngine;

public class MeteorEnemy : MonoBehaviour
{
    public float minForceX = 4f;
    public float maxForceX = 8f;
    public float minForceY = 8f;
    public float maxForceY = 12f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Randomly choose left or right spawn
        bool fromLeft = transform.position.x < 0;

        // Generate random force
        float forceX = Random.Range(minForceX, maxForceX);
        float forceY = Random.Range(minForceY, maxForceY);

        // Flip direction if from right
        if (!fromLeft) forceX = -forceX;

        // Apply force to simulate jump
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
    }

    void Update()
    {
        // Destroy when off screen
        if (transform.position.y < -60f)
        {
            Destroy(gameObject);
        }
    }
}
