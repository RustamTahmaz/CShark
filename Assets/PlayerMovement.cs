using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Adjust movement speed
    public float moveLimit = 2.5f; // Prevent player from moving too far left/right

    void Update()
    {
        float moveX = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Restrict movement within screen bounds
        float clampedX = Mathf.Clamp(transform.position.x + moveX, -moveLimit, moveLimit);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
