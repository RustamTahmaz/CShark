using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int health = 1;

    public abstract void TakeDamage(int damage);
    
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
} 