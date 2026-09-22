using System.Collections;
using UnityEngine;

public class PlayerFlicker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Flicker Settings")]
    [Tooltip("Intervalo en segundos entre cada parpadeo")]
    [SerializeField] private float flickerInterval = 0.08f;

    [Tooltip("Nivel de transparencia durante el parpadeo (0 = invisible, 1 = opaco)")]
    [Range(0f, 1f)]
    [SerializeField] private float minAlpha = 0.2f;

    private Coroutine _flickerCoroutine;
    private Color _originalColor;

    private void Awake()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            _originalColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnInvincibilityStarted += StartFlicker;
            playerHealth.OnInvincibilityEnded += StopFlicker;
            playerHealth.OnDeath += StopFlicker;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnInvincibilityStarted -= StartFlicker;
            playerHealth.OnInvincibilityEnded -= StopFlicker;
            playerHealth.OnDeath -= StopFlicker;
        }
    }

    private void StartFlicker(float duration)
    {
        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);
        _flickerCoroutine = StartCoroutine(FlickerRoutine(duration));
    }

    private void StopFlicker()
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
        }
        ResetColor();
    }

    private IEnumerator FlickerRoutine(float duration)
    {
        float elapsed = 0f;
        bool isLowAlpha = false;

        while (elapsed < duration)
        {
            isLowAlpha = !isLowAlpha;

            if (spriteRenderer != null)
            {
                Color color = _originalColor;
                color.a = isLowAlpha ? minAlpha : _originalColor.a;
                spriteRenderer.color = color;
            }

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        ResetColor();
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = _originalColor;
        }
    }
}
