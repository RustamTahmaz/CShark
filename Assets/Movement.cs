using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Speed in world units per second")]
    public float speed = 5f;
    
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // -1..1
        float moveY = Input.GetAxis("Vertical");   // -1..1

        // Construct a direction vector
        Vector2 moveDirection = new Vector2(moveX, moveY);

        // Move the character by direction * speed * deltaTime
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}
