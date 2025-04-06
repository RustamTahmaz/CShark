using UnityEngine;

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

    private bool isDashing = false;

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

                // Debug info
                Debug.Log(
                    $"[Swipe End] Dist: {verticalDist}, Duration: {swipeDuration}, " +
                    $"isVerticalSwipe={isVerticalSwipe}, isQuickSwipe={isQuickSwipe}"
                );

                if (isVerticalSwipe && isQuickSwipe && !isDashing)
                {
                    float dir = (verticalDist > 0) ? 1f : -1f;
                    StartCoroutine(DashVerticalCoroutine(dir));
                }
                break;
            }
        }
    }

    private System.Collections.IEnumerator DashVerticalCoroutine(float dir)
    {
        isDashing = true;
        // animator.SetBool("IsDashing", true);

        // The dash’s starting Y and final (clamped) Y
        float startY = transform.position.y;
        // We still compute dashTarget as before, but we only use its Y.
        Vector3 dashTarget = transform.position + new Vector3(0f, dir * verticalDashDistance, 0f);
        dashTarget = ClampToCamera(dashTarget); // for safety, though we only really need dashTarget.y

        float targetY = dashTarget.y;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;

            // 1) Keep the current X (in case the user is dragging horizontally)
            float currentX = transform.position.x;

            // 2) Lerp just the Y coordinate
            float newY = Mathf.Lerp(startY, targetY, t);

            // 3) Construct the new position
            Vector3 newPos = new Vector3(currentX, newY, transform.position.z);

            // 4) Clamp the new position so we don't leave the screen (including the X if needed)
            newPos = ClampToCamera(newPos);

            // 5) Assign to transform
            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final snap to the fully dashed Y (just to ensure consistency)
        // but keep the current X
        float finalX = transform.position.x;
        Vector3 finalPos = new Vector3(finalX, targetY, transform.position.z);
        finalPos = ClampToCamera(finalPos);
        transform.position = finalPos;

        isDashing = false;
        // animator.SetBool("IsDashing", false);
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

        // Debug info to see clamping bounds
        // You can comment out these logs after you confirm the values
        // are what you expect.
        // They will show you the "targetPos" vs. "clamped" position.
        Debug.Log(
            $"[Clamp] minY={minY}, maxY={maxY}, " +
            $"targetY={targetPos.y}, resultY={Mathf.Clamp(targetPos.y, minY, maxY)}"
        );

        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
        return new Vector3(clampedX, clampedY, targetPos.z);
    }

    
}
