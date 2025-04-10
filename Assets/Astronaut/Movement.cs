using UnityEngine;
using System.Collections;

public class KnightmareSwipeMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [Tooltip("Scale factor for translating screen-pixel drag into world movement.")]
    public float horizontalDragFactor = 0.02f;

    [Tooltip("Maximum horizontal speed if you want to limit it (optional).")]
    public float maxHorizontalSpeed = 10f;

    [Header("Vertical Dash")]
    [Tooltip("Distance (in world units) of the upward/downward dash.")]
    public float verticalDashDistance = 50f;

    [Tooltip("Minimum vertical distance (in screen pixels) to register a dash.")]
    public float verticalSwipeThreshold = 80f;

    [Tooltip("Maximum time (in seconds) to consider it a 'flick' for dashing.")]
    public float swipeTimeThreshold = 0.3f;

    [Tooltip("Time (in seconds) to complete the dash movement.")]
    public float dashDuration = 0.4f;

    [Header("Camera Clamping")]
    [Tooltip("Padding so the character doesn't clip exactly on screen edges.")]
    public float boundaryPadding = 2.5f;

    private Camera mainCam;
    private Vector2 lastTouchPos;
    private Vector2 startTouchPos;
    private float touchStartTime;

    // Tracks if we are currently in the middle of a dash
    private bool isDashing = false;
    // Which way we’re currently dashing: +1 for up, -1 for down
    private float currentDashDirection = 0f;
    // Reference to the active dash coroutine, so we can stop it mid-dash
    private Coroutine dashRoutine;

    private void Awake()
    {
        mainCam = Camera.main;
        if (!mainCam)
        {
            Debug.LogError("No main camera found! Make sure the camera has 'MainCamera' tag.");
        }
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                startTouchPos = touch.position;
                lastTouchPos  = touch.position;
                touchStartTime = Time.time;
                break;

            case TouchPhase.Moved:
                // If you do NOT want to allow horizontal movement during dash, uncomment:
                // if (isDashing) break;

                float deltaX = touch.position.x - lastTouchPos.x;
                float moveX  = deltaX * horizontalDragFactor;

                // Move the character horizontally and clamp
                Vector3 newPos = transform.position + new Vector3(moveX, 0f, 0f);
                transform.position = ClampToCamera(newPos);

                lastTouchPos = touch.position;
                break;

            case TouchPhase.Ended:
            {
                Vector2 endTouchPos   = touch.position;
                float   swipeDuration = Time.time - touchStartTime;
                float   verticalDist  = endTouchPos.y - startTouchPos.y;

                bool isVerticalSwipe = Mathf.Abs(verticalDist) > verticalSwipeThreshold;
                bool isQuickSwipe    = swipeDuration <= swipeTimeThreshold;

                Debug.Log(
                    $"[Swipe End] Dist: {verticalDist}, Duration: {swipeDuration}, " +
                    $"isVerticalSwipe={isVerticalSwipe}, isQuickSwipe={isQuickSwipe}"
                );

                // Only start a dash if it's a valid quick vertical swipe and we're not already dashing
                if (isVerticalSwipe && isQuickSwipe && !isDashing)
                {
                    float dir = (verticalDist > 0) ? 1f : -1f;
                    StartDash(dir);
                }
                break;
            }
        }
    }

    /// <summary>
    /// Public method to start a dash in a given direction.
    /// If a dash is already happening, we stop it immediately and begin the new dash.
    /// </summary>
    private void StartDash(float dir)
    {
        // If a dash coroutine is currently running, stop it
        if (dashRoutine != null)
        {
            StopCoroutine(dashRoutine);
            dashRoutine = null;
        }

        dashRoutine = StartCoroutine(DashVerticalCoroutine(dir));
    }

    private IEnumerator DashVerticalCoroutine(float dir)
    {
        isDashing = true;
        currentDashDirection = dir;

        // The dash’s starting Y
        float startY = transform.position.y;

        // We compute the final Y but clamp it
        Vector3 dashTarget = transform.position + new Vector3(0f, dir * verticalDashDistance, 0f);
        dashTarget = ClampToCamera(dashTarget); 
        float targetY = dashTarget.y;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;

            float currentX = transform.position.x; // preserve horizontal
            float newY = Mathf.Lerp(startY, targetY, t);

            Vector3 newPos = new Vector3(currentX, newY, transform.position.z);
            newPos = ClampToCamera(newPos);
            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final snap
        float finalX = transform.position.x;
        Vector3 finalPos = new Vector3(finalX, targetY, transform.position.z);
        finalPos = ClampToCamera(finalPos);
        transform.position = finalPos;

        isDashing = false;
        dashRoutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only do something if the player is mid-dash
        if (!isDashing) return;

        if (other.CompareTag("Meteor"))
        {
            // Destroy the meteor
            Destroy(other.gameObject);

            // Immediately interrupt the current dash
            if (dashRoutine != null)
            {
                StopCoroutine(dashRoutine);
                dashRoutine = null;
            }
            isDashing = false;

            // Reverse dash direction: if we were dashing down (-1), dash up (+1), etc.
            float reverseDir = (currentDashDirection > 0f) ? -1f : 1f;

            // Start a new dash from the collision point
            StartDash(reverseDir);
        }
    }

    private Vector3 ClampToCamera(Vector3 targetPos)
    {
        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth  = camHeight * mainCam.aspect;
        Vector3 camCenter = mainCam.transform.position;

        float minX = camCenter.x - camWidth  / 2f + boundaryPadding;
        float maxX = camCenter.x + camWidth  / 2f - boundaryPadding;
        float minY = camCenter.y - camHeight / 2f + boundaryPadding + 3f;
        float maxY = camCenter.y + camHeight / 2f - boundaryPadding - 4f;

        Debug.Log(
            $"[Clamp] minY={minY}, maxY={maxY}, " +
            $"targetY={targetPos.y}, resultY={Mathf.Clamp(targetPos.y, minY, maxY)}"
        );

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
        return new Vector3(clampedX, clampedY, targetPos.z);
    }
}
