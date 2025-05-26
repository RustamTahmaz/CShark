using UnityEngine;

public class PowerUpCollectEffect : MonoBehaviour
{
    public float lifetime = 0.5f;
    public float expandSpeed = 2f;
    public float fadeSpeed = 2f;

    private SpriteRenderer spriteRenderer;
    private Color startColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Expand
        transform.localScale += Vector3.one * expandSpeed * Time.deltaTime;

        // Fade out
        Color currentColor = spriteRenderer.color;
        currentColor.a = Mathf.Lerp(currentColor.a, 0f, fadeSpeed * Time.deltaTime);
        spriteRenderer.color = currentColor;
    }
} 