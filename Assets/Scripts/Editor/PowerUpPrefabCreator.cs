using UnityEngine;
using UnityEditor;

public class PowerUpPrefabCreator : EditorWindow
{
    private GameObject powerUpPrefab;
    private Sprite powerUpSprite;
    private Color powerUpColor = new Color(1f, 0.92f, 0.016f, 1f); // Yellow for invincibility
    private float colliderRadius = 0.5f;
    private float rotationSpeed = 100f;
    private float bobSpeed = 2f;
    private float bobHeight = 0.2f;

    [MenuItem("Tools/Power-up Prefab Creator")]
    public static void ShowWindow()
    {
        GetWindow<PowerUpPrefabCreator>("Power-up Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Power-up Prefab", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        powerUpSprite = (Sprite)EditorGUILayout.ObjectField("Power-up Sprite", powerUpSprite, typeof(Sprite), false);
        powerUpColor = EditorGUILayout.ColorField("Power-up Color", powerUpColor);
        colliderRadius = EditorGUILayout.FloatField("Collider Radius", colliderRadius);
        rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", rotationSpeed);
        bobSpeed = EditorGUILayout.FloatField("Bob Speed", bobSpeed);
        bobHeight = EditorGUILayout.FloatField("Bob Height", bobHeight);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Power-up Prefab"))
        {
            CreatePowerUpPrefab();
        }
    }

    private void CreatePowerUpPrefab()
    {
        if (powerUpSprite == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a sprite for the power-up.", "OK");
            return;
        }

        // Create the power-up GameObject
        GameObject powerUp = new GameObject("PowerUp");
        
        // Add components
        SpriteRenderer spriteRenderer = powerUp.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = powerUp.AddComponent<CircleCollider2D>();
        PowerUpItem powerUpItem = powerUp.AddComponent<PowerUpItem>();

        // Configure components
        spriteRenderer.sprite = powerUpSprite;
        spriteRenderer.color = powerUpColor;
        collider.isTrigger = true;
        collider.radius = colliderRadius;

        // Configure PowerUpItem
        powerUpItem.rotationSpeed = rotationSpeed;
        powerUpItem.bobSpeed = bobSpeed;
        powerUpItem.bobHeight = bobHeight;

        // Create prefab
        string prefabPath = "Assets/Prefabs/PowerUp.prefab";
        CreatePrefabFolderIfNeeded();
        PrefabUtility.SaveAsPrefabAsset(powerUp, prefabPath);
        DestroyImmediate(powerUp);

        EditorUtility.DisplayDialog("Success", "Power-up prefab created successfully!", "OK");
    }

    private void CreatePrefabFolderIfNeeded()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
    }
} 