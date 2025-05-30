using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 12f;
    public float verticalDashDistance = 50f;
    public float verticalSwipeThreshold = 80f;
    public float swipeTimeThreshold = 0.3f;
    public float dashDuration = 0.2f;
    public float boundaryPadding = 2.5f;
    public float bounceForce = 30f;
    public float gravityScale = 0.5f;
    public float maxRisingBorder = 15f; // Maximum Y position (top border)

    [Header("Power-up Visuals")]
    public GameObject invincibilityEffect;
    public GameObject speedBoostEffect;
    public GameObject doublePointsEffect;
    public GameObject GameOverPanel;
     
    // public TextMeshProUGUI finalScoreLabel;
    // public TextMeshProUGUI bestScoreLabel;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 touchStartPos;
    private float touchStartTime;
    private bool isDashing = false;
    private bool isDashingDown = false;
    private bool hitEnemyDuringDash = false;
    private Coroutine dashRoutine;
    private PowerUpManager powerUpManager;
    private bool isGameOver = false;
    private float targetX;
    private float moveSmoothTime = 0.02f;
    private float currentVelocityX;
    private bool isMoving = false;

    [Header("VFX & Extras")]
    public GameObject meteorExplosionPrefab;  // drag in a simple explosion ParticleSystem prefab

    [Header("Health UI")]
    public Image           healthIcon;  // drag in your HealthIcon here
    public TextMeshProUGUI healthText;  // drag in your HealthText here

    // [Header("Base Stats for Upgrades")]
    // public int   baseMaxHealth      = 3;   // your default starting lives
    // public float baseMoveSpeed      = 12f; // your default horizontal speed
    // public float dashDistance   = 50f; // your default vertical dash distance
    // public float baseRocketSpeed    = 10f; // if you ever implement a rocket boost
    // public float rocketSpeed    = 10f; // if you ever implement a rocket boost

    private int currentHealth;
    public int maxHealth = 3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
        mainCam = Camera.main;

        currentHealth = maxHealth;
        UpdateHealthUI();
        
        if (rb != null)
        {
            rb.gravityScale = 0f; // No gravity by default
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.mass = 1f;
            rb.linearDamping = 0.5f;
            rb.angularDamping = 0f;
        }
        if (powerUpManager == null)
        {
            Debug.LogError("PowerUpManager not found in the scene!");
        }
        // IMPORTANT: Make sure there is only one UIManager and one ScoreManager in your scene to avoid duplicate score texts.
    }

    private void Start()
    {
        if (invincibilityEffect) invincibilityEffect.SetActive(false);
        if (speedBoostEffect) speedBoostEffect.SetActive(false);
        if (doublePointsEffect) doublePointsEffect.SetActive(false);

        // var up = UpgradeManager.Instance;

        // // Armor gives you extra maxHealth per level
        // maxHealth = baseMaxHealth + up.GetArmorLevel();

        // // Gloves could boost your dashDamage or baseScorePerHit:
        // ScoreManager.Instance.baseScorePerHit *= 1f + 0.1f * up.GetGloveLevel();

        // // Boots increase your horizontal speed:
        // moveSpeed *= 1f + 0.1f * up.GetBootLevel();
        // dashDistance *= 1f + 0.1f * up.GetBootLevel();   // optional

        // // Rocket might affect initial upward boost or similar
        // rocketSpeed = baseRocketSpeed * (1f + 0.1f * up.GetRocketLevel());
    }

    private void Update()
    {
        if (isGameOver) return;

        HandleInput();
        UpdateMovement();
        CheckBoundaries();
        UpdatePowerUpEffects();

        // If dashing down and not hit enemy, check for game over
        if (isDashingDown && !hitEnemyDuringDash)
        {
            float screenHeight = Camera.main.orthographicSize;
            float bottomBoundary = -screenHeight - 18f;
            if (transform.position.y < bottomBoundary)
            {
                GameOver();
            }
        }
    }


    public void TakeDamage(int amount = 1)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHealthUI();
        // Optional: update any health‐UI here
        AudioManager.Instance.PlaySfx(AudioManager.Instance.hurtSfx);
        if (currentHealth <= 0)
        {
            
            GameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"× {currentHealth}";
        // (Optionally) you can hide the icon at 0 health:
        // if (currentHealth == 0) healthIcon.enabled = false;
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    touchStartTime = Time.time;
                    isMoving = true;
                    break;
                case TouchPhase.Moved:
                    // if (!isDashing)
                    {
                        Vector3 touchWorldPos = mainCam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -mainCam.transform.position.z));
                        targetX = touchWorldPos.x;
                    }
                    break;
                case TouchPhase.Ended:
                    isMoving = false;
                    float touchDuration = Time.time - touchStartTime;
                    Vector2 touchDelta = touch.position - touchStartPos;
                    if (touchDuration <= swipeTimeThreshold && Mathf.Abs(touchDelta.y) >= verticalSwipeThreshold)
                    {
                        if (touchDelta.y < 0)
                        {
                            // Downward dash only
                            StartDash(Vector2.down);
                        }
                        // Swipe up is ignored/removed
                    }
                    break;
            }
        }
    }

    private void StartDash(Vector2 direction)
    {
        if (!isDashing)
        {
            isDashing = true;
            if (dashRoutine != null)
            {
                StopCoroutine(dashRoutine);
            }
            dashRoutine = StartCoroutine(DashCoroutine(direction));
        }
    }

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        float dashSpeed = verticalDashDistance / dashDuration;
        if (direction == Vector2.down)
        {
            isDashingDown = true;
            hitEnemyDuringDash = false;
        }
        else
        {
            isDashingDown = false;
            hitEnemyDuringDash = false;
        }
        if (rb != null)
        {
            rb.gravityScale = 0f; // No gravity during dash
            rb.linearVelocity = direction * dashSpeed;
        }
        yield return new WaitForSeconds(dashDuration);
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            // Only enable gravity if dashing down and missed all enemies
            if (isDashingDown && !hitEnemyDuringDash)
            {
                rb.gravityScale = gravityScale; // Start falling
            }
            else
            {
                rb.gravityScale = 0f; // Stay in place
            }
        }
        isDashing = false;
        // If dash was downward and no enemy was hit, let Update() handle game over by falling
        if (!isDashingDown)
        {
            isDashingDown = false;
            hitEnemyDuringDash = false;
        }
    }

    private void UpdateMovement()
    {
        if (isMoving)
        {
            float newX = Mathf.SmoothDamp(
                transform.position.x,
                targetX,
                ref currentVelocityX,
                moveSmoothTime
            );
            Vector3 newPos = new Vector3(newX, transform.position.y, transform.position.z);
            transform.position = ClampToCamera(newPos);
        }
        else
        {
            currentVelocityX = 0f;
        }
    }


    private void CheckBoundaries()
    {
        Vector3 pos = transform.position;

        float halfH   = mainCam.orthographicSize;
        float halfW   = halfH * mainCam.aspect;
        float centerY = mainCam.transform.position.y;

        // X clamp
        pos.x = Mathf.Clamp(pos.x,
            -halfW + boundaryPadding,
             halfW - boundaryPadding
        );

        // Y clamp: camera bottom/top with padding, capped by maxRisingBorder
        float minY = centerY - halfH + boundaryPadding;
        float maxY = centerY + halfH - boundaryPadding - 5f;
        if (maxY > maxRisingBorder) maxY = maxRisingBorder;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Meteor"))
            return;

        var meteor = collision.gameObject;

        if (isDashing)  
        {
            // ─── You dashed into it ──────────────────
            // play your bounce, score and destroy:
            hitEnemyDuringDash = true;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            rb.gravityScale = 0f;

            isDashing = isDashingDown = false;
            if (dashRoutine != null) StopCoroutine(dashRoutine);

            ScoreManager.Instance?.AddScoreOnHit();
            // CoinManager.Instance.AddCoins(1);

            // optional: explosion VFX  
            if (meteorExplosionPrefab != null)
                Instantiate(meteorExplosionPrefab, meteor.transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfx(AudioManager.Instance.boomSfx);
            Destroy(meteor);
            
        }
        else  
        {
            // ─── You walked into it ─────────────────
            TakeDamage(1);

            // optional: small hit VFX or camera shake
            if (meteorExplosionPrefab != null)
                Instantiate(meteorExplosionPrefab, meteor.transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfx(AudioManager.Instance.boomSfx);
            Destroy(meteor);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Meteor logic
        if (other.CompareTag("Meteor"))
        {
            GameObject meteor = other.gameObject;

            // If you dashed into it, bounce & score
            if (isDashing)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                rb.gravityScale = 0f;
                isDashing = isDashingDown = false;
                if (dashRoutine != null) StopCoroutine(dashRoutine);

                ScoreManager.Instance?.AddScoreOnHit();
                // CoinManager.Instance.AddCoins(1);
            }
            else
            {
                // Otherwise take damage
                TakeDamage(1);
            }

            AudioManager.Instance.PlaySfx(AudioManager.Instance.boomSfx);
            Destroy(meteor);
            
            return;
        }

        // 2) UFO logic
        if (other.CompareTag("UFO"))
        {
            GameObject ufo = other.gameObject;

            if (isDashing)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                rb.gravityScale = 0f;
                isDashing = isDashingDown = false;
                if (dashRoutine != null) StopCoroutine(dashRoutine);

                ScoreManager.Instance?.AddScoreOnHit();
                // CoinManager.Instance.AddCoins(2);
            }
            else
            {
                TakeDamage(1);
            }

            AudioManager.Instance.PlaySfx(AudioManager.Instance.boomSfx);
            Destroy(ufo);
            
            return;
        }
    }



    private void GameOver()
    {
        
        if (isGameOver) return;
        isGameOver = true;

        // GameObject.Find("UIContainer")?.SetActive(false);

        if (GameOverPanel != null)
        GameOverPanel.SetActive(true);


        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveHighScore();
        }

        // GameOverPanel.SetActive(true);
        // GameOverPanel.transform.SetAsLastSibling();
        
        Time.timeScale = 0f;
        // SceneManager.LoadScene(0);
    }

    private void UpdatePowerUpEffects()
    {
        if (powerUpManager == null) return;
        if (invincibilityEffect)
            invincibilityEffect.SetActive(powerUpManager.IsInvincible());
        if (speedBoostEffect)
            speedBoostEffect.SetActive(powerUpManager.HasSpeedBoost());
        if (doublePointsEffect)
            doublePointsEffect.SetActive(powerUpManager.HasDoublePoints());
    }

    private Vector3 ClampToCamera(Vector3 targetPos)
    {
        float camHeight = mainCam.orthographicSize * 2f;
        float camWidth = camHeight * mainCam.aspect;
        Vector3 camCenter = mainCam.transform.position;
        float minX = camCenter.x - camWidth / 2f + boundaryPadding;
        float maxX = camCenter.x + camWidth / 2f - boundaryPadding;
        float minY = camCenter.y - camHeight / 2f + boundaryPadding + 3f;
        float maxY = Mathf.Min(camCenter.y + camHeight / 2f - boundaryPadding - 4f, maxRisingBorder); // Clamp to maxRisingBorder
        float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
        return new Vector3(clampedX, clampedY, targetPos.z);
    }

    // private Vector3 ClampToCamera(Vector3 targetPos)
    // {
    //     float halfH = mainCam.orthographicSize;
    //     float halfW = halfH * mainCam.aspect;
    //     Vector3 c   = mainCam.transform.position;

    //     float minX = c.x - halfW + boundaryPadding;
    //     float maxX = c.x + halfW - boundaryPadding;

    //     float minY = c.y - halfH + boundaryPadding;
    //     float maxY = c.y + halfH - boundaryPadding;
    //     if (maxY > maxRisingBorder) maxY = maxRisingBorder;

    //     float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
    //     float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
    //     return new Vector3(clampedX, clampedY, targetPos.z);
    // }
} 