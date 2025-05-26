using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    public float baseScorePerHit = 10f;
    public float comboMultiplier = 1.2f;
    public float comboTimeWindow = 2f;
    public float maxComboMultiplier = 3f;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private float currentScore;
    private float highScore;
    private int currentCombo = 0;
    private float comboTimeLeft;
    private float currentMultiplier = 1f;
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene is loaded (including your Restart)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        // Game-Over panel fallbacks
        if (scoreText == null)
            scoreText = GameObject.Find("FinalScoreText")?
                            .GetComponent<TextMeshProUGUI>();
        if (highScoreText == null)
            highScoreText = GameObject.Find("BestScoreText")?
                                .GetComponent<TextMeshProUGUI>();

        // Immediately refresh their values
        UpdateScoreDisplay();
        UpdateHighScoreDisplay();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadHighScore();
    }


    private void Start()
    {
            // Live HUD → GameOver panel fallback
        if (scoreText == null)
        {
            scoreText = GameObject.Find("FinalScoreText")?
                            .GetComponent<TextMeshProUGUI>();
        }
        if (highScoreText == null)
        {
            highScoreText = GameObject.Find("BestScoreText")?
                                .GetComponent<TextMeshProUGUI>();
        }

        UpdateScoreDisplay();
        UpdateHighScoreDisplay();
    }

    private void Update()
    {
        if (comboTimeLeft > 0)
        {
            comboTimeLeft -= Time.deltaTime;
            if (comboTimeLeft <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void AddScoreOnHit()
    {
        // Reset combo timer or extend it
        comboTimeLeft = comboTimeWindow;

        // Increase combo
        currentCombo++;
        
        // Calculate score with combo multiplier
        float scoreToAdd = baseScorePerHit * currentMultiplier;
        currentScore += scoreToAdd;

        // Update multiplier for next hit
        currentMultiplier = Mathf.Min(currentMultiplier * comboMultiplier, maxComboMultiplier);

        // Update UI
        UpdateScoreDisplay();
        
        // Show combo if available
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCombo(currentCombo);
        }

        // Check for new high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            UpdateHighScoreDisplay();
        }

        Debug.Log($"Score added: {scoreToAdd}, Total: {currentScore}, Combo: {currentCombo}x");
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        currentMultiplier = 1f;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCombo(0);
        }
    }

    private void UpdateScoreDisplay()
    {
        int dispScore = Mathf.FloorToInt(currentScore);
        
        if (scoreText != null)
        {
            scoreText.text = $"{dispScore}";
        }
        else
        {
            Debug.LogWarning("Score Text UI element not found!");
        }

        // ───── NEW: Sync with UIManager’s HUD ──────────────────────
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreText($"Score: {dispScore}");
        }
    }

    private void UpdateHighScoreDisplay()
    {
        int dispHigh = Mathf.FloorToInt(highScore);

        if (highScoreText != null)
        {
            highScoreText.text = $"{dispHigh}";
        }
        else
        {
            Debug.LogWarning("High Score Text UI element not found!");
        }

        // ───── NEW: Sync with UIManager’s HUD ──────────────────────
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHighScoreText($"Best: {dispHigh}");
        }
    }


    public void SaveHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
        Debug.Log($"High score saved: {highScore}");
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        Debug.Log($"High score loaded: {highScore}");
    }

    public void ResetScore()
    {
        currentScore = 0f;
        currentCombo = 0;
        currentMultiplier = 1f;
        comboTimeLeft = 0f;
        UpdateScoreDisplay();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowCombo(0);
        }
        scoreText = GameObject.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find("BestScoreText")?.GetComponent<TextMeshProUGUI>();
    }

    public float GetCurrentScore()
    {
        return currentScore;
    }

    public float GetHighScore()
    {
        return highScore;
    }
} 