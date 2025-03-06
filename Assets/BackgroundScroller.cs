using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("Scrolling speed (world units/second). Positive = up, negative = down.")]
    [SerializeField] private float scrollSpeed = -50f;

    private BoxCollider2D _collider;
    private Rigidbody2D _rb;

    // The total height of the background in world units.
    private float _backgroundHeight;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rb       = GetComponent<Rigidbody2D>();

        // If you don’t need collisions, disable the collider
        _collider.enabled = false;

        // Calculate the real world‐space height:
        //   collider size Y * the transform's local scale.
        _backgroundHeight = _collider.size.y * transform.localScale.y;

        // Use Rigidbody2D velocity for movement
        _rb.linearVelocity = new Vector2(0f, scrollSpeed);
        // Make sure RigidBody2D is Kinematic (or Dynamic if that suits your design).
    }

    // Use FixedUpdate to stay in sync with physics movement
    private void FixedUpdate()
    {
        // If this background’s center is now below (or above) the “threshold”
        // we pop it back up (or down) by 2 x height.
        // Because it’s scrolled in the negative Y direction,
        // the check is (position < -_backgroundHeight).
        if (transform.position.y < -_backgroundHeight)
        {
            transform.position = new Vector2(
                transform.position.x,
                transform.position.y + 1.5f * _backgroundHeight
            );
        }
    }
}
