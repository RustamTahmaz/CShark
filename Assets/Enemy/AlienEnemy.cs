using UnityEngine;

public class AlienEnemy : EnemyBase
{
    public float speed = 5f;
    public float amplitude = 2f;
    public float frequency = 2f;
    private Vector3 startPos;
    private float direction;

    void Start()
    {
        startPos = transform.position;
        direction = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position += new Vector3(direction * speed * Time.deltaTime, y * Time.deltaTime, 0);
        if (transform.position.y < -60f)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
} 