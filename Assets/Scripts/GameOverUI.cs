using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Assign in Inspector or auto-find")]
    public GameObject      GameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        // Auto-find the panel if you forgot to assign it
        if (GameOverPanel == null)
        {
            GameOverPanel = GameObject.Find("GameOverPanel");
            if (GameOverPanel == null)
                Debug.LogError("GameOverUI: GameOverPanel not assigned and not found in scene!");
        }
    }

    private void Start()
    {
        // Guarantee it’s hidden at the start
        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Call this when the player dies.
    /// </summary>
    public void ShowPanel()
    {
        if (GameOverPanel == null)
        {
            Debug.LogError("ShowPanel(): GameOverPanel is null!");
            return;
        }

        // Grab scores
        var sm = ScoreManager.Instance;
        int finalScore = sm != null ? Mathf.FloorToInt(sm.GetCurrentScore()) : 0;
        int bestScore  = sm != null ? Mathf.FloorToInt(sm.GetHighScore())   : 0;

        // Update your panel’s text
        if (finalScoreText != null) finalScoreText.text = $"Final Score: {finalScore}";
        if (bestScoreText  != null) bestScoreText .text = $"Best Score:  {bestScore}";

        // Show it and push to front
        GameOverPanel.SetActive(true);
        GameOverPanel.transform.SetAsLastSibling();
    }

    // (Buttons…)
    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnMenuPressed()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene("Menu");
    }
    public void OnStorePressed()
    {
        Time.timeScale = 1f;
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene("Store");
    }
}
