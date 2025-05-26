using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public Button storeButton;
    public Button quitButton;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        // Setup button listeners
        if (playButton != null)
        {
            playButton.onClick.AddListener(StartGame);
        }

        if (storeButton != null)
        {
            storeButton.onClick.AddListener(OpenStore);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitApplication);
        }

        UpdateHighScoreDisplay();
    }

    private void UpdateHighScoreDisplay()
    {
        if (highScoreText != null)
        {
            float highScore = PlayerPrefs.GetFloat("HighScore", 0f);
            highScoreText.text = $"High Score: {Mathf.FloorToInt(highScore)}";
        }
    }

    public void StartGame()
    {
        // Load the game scene (now at index 1)
        SceneManager.LoadScene(1);
    }

    public void OpenStore()
    {
        // Load the store scene (at index 2)
        SceneManager.LoadScene(2);
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 