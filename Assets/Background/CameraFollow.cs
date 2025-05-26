using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Assign the Player object in the Inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, player.position.y + offset.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}
