using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI powerUpStatusText;
    private TextMeshProUGUI comboText;

    [Header("UI Settings")]
    public int baseFontSize = 24; // Reduced base font size
    public Color textColor = Color.white;
    public Color powerUpActiveColor = Color.yellow;
    public Color comboColor = new Color(1f, 0.5f, 0f);

    [Header("Responsive Settings")]
    public float minFontSize = 18f; // Reduced minimum font size
    public float maxFontSize = 32f; // Reduced maximum font size
    public float screenWidthThreshold = 720f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Ensure UI is set up when the object is enabled
        if (scoreText == null || highScoreText == null || powerUpStatusText == null || comboText == null)
        {
            SetupUI();
        }
    }

    private void SetupUI()
    {
        Debug.Log("Setting up UI...");

        // Create Canvas if it doesn't exist
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Setup Canvas Scaler
            CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            // Add Raycaster
            gameObject.AddComponent<GraphicRaycaster>();
        }

        // Create UI Container
        GameObject uiContainer = new GameObject("UIContainer");
        uiContainer.transform.SetParent(transform, false);
        RectTransform containerRect = uiContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        // Calculate responsive sizes
        float fontSize = CalculateResponsiveFontSize();
        float leftMargin = 0.02f; // 2% from left
        float topMargin = 0.02f; // 2% from top
        float elementHeight = 0.04f; // 4% of screen height
        float elementWidth = 0.2f; // 20% of screen width
        float elementSpacing = 0.01f; // 1% spacing

        // Create Score Text
        scoreText = CreateTextElement(uiContainer.transform, "ScoreText", 
            new Vector2(leftMargin + elementWidth/2, 1f - topMargin - elementHeight/2), 
            new Vector2(elementWidth, elementHeight));
        scoreText.text = "Score: 0";
        scoreText.alignment = TextAlignmentOptions.Left;
        scoreText.fontSize = fontSize;
        Debug.Log("Created Score Text");

        // Create High Score Text
        highScoreText = CreateTextElement(uiContainer.transform, "HighScoreText", 
            new Vector2(leftMargin + elementWidth/2, 1f - topMargin - elementHeight*1.5f - elementSpacing), 
            new Vector2(elementWidth, elementHeight));
        highScoreText.text = "Best: 0";
        highScoreText.alignment = TextAlignmentOptions.Left;
        highScoreText.fontSize = fontSize;
        Debug.Log("Created High Score Text");

        // Create Power-up Status Text
        powerUpStatusText = CreateTextElement(uiContainer.transform, "PowerUpText", 
            new Vector2(leftMargin + elementWidth/2, 1f - topMargin - elementHeight*2.5f - elementSpacing*2), 
            new Vector2(elementWidth, elementHeight));
        powerUpStatusText.text = "Power-ups: None";
        powerUpStatusText.alignment = TextAlignmentOptions.Left;
        powerUpStatusText.fontSize = fontSize;
        Debug.Log("Created Power-up Status Text");

        // Create Combo Text
        comboText = CreateTextElement(uiContainer.transform, "ComboText", 
            new Vector2(0.5f, 0.5f), 
            new Vector2(0.2f, elementHeight * 1.5f));
        comboText.text = "COMBO!";
        comboText.alignment = TextAlignmentOptions.Center;
        comboText.fontSize = fontSize * 1.2f;
        comboText.gameObject.SetActive(false);
        Debug.Log("Created Combo Text");

        // Add safe area padding
        AddSafeAreaPadding(uiContainer);

        Debug.Log("UI Setup Complete");
    }

    private float CalculateResponsiveFontSize()
    {
        float screenWidth = Screen.width;
        float scaleFactor = Mathf.Clamp(screenWidth / screenWidthThreshold, 0.5f, 1.5f);
        return Mathf.Clamp(baseFontSize * scaleFactor, minFontSize, maxFontSize);
    }

    private void AddSafeAreaPadding(GameObject container)
    {
        RectTransform rectTransform = container.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }

    private TextMeshProUGUI CreateTextElement(Transform parent, string name, Vector2 position, Vector2 size)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent, false);

        RectTransform rectTransform = textObj.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(position.x - size.x/2, position.y - size.y/2);
        rectTransform.anchorMax = new Vector2(position.x + size.x/2, position.y + size.y/2);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.color = textColor;
        tmp.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.overflowMode = TextOverflowModes.Overflow;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = minFontSize;
        tmp.fontSizeMax = maxFontSize;
        
        return tmp;
    }

    public void UpdateScoreText(string text)
    {
        if (scoreText != null)
        {
            scoreText.text = text;
        }
    }

    public void UpdateHighScoreText(string text)
    {
        if (highScoreText != null)
        {
            highScoreText.text = text;
        }
    }

    public void UpdatePowerUpStatus(string status)
    {
        if (powerUpStatusText != null)
        {
            powerUpStatusText.text = status;
        }
    }

    public void ShowCombo(int combo)
    {
        if (comboText != null)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = $"{combo}x COMBO!";
            StartCoroutine(AnimateComboText());
        }
    }

    private IEnumerator AnimateComboText()
    {
        if (comboText != null)
        {
            comboText.transform.localScale = Vector3.one * 1.2f;
            float duration = 0.2f;
            float elapsed = 0f;
            Vector3 startScale = comboText.transform.localScale;
            Vector3 targetScale = Vector3.one;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                comboText.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            comboText.transform.localScale = targetScale;
            yield return new WaitForSeconds(1f);
            comboText.gameObject.SetActive(false);
        }
    }
} 