using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Tooltip("Seconds before this meteor auto-deletes")]
    public float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
