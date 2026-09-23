using System;
using System.Collections;
using UnityEngine;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Config")]
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float invincibilityDuration = 0.5f;
    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsInvulnerable { get; private set; }
    #region Events
    public event Action OnDeath;
    // Eventos para el efecto visual de invulnerabilidad
    public event Action<float> OnInvincibilityStarted; // Envía la duración
    public event Action OnInvincibilityEnded;
    #endregion
    private void Awake()
    {
        CurrentHealth = maxHealth;
    }
    private void Start()
    {
        EventManager.TriggerEvent(EventType.PlayerHealthChanged, CurrentHealth, maxHealth);
    }
    public void TakeDamage(float amount, string attackerName = "Unknown")
    {
        if (IsInvulnerable || CurrentHealth <= 0f) return;

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);

        Debug.Log($"<color=red>[Damage]</color> Player took {amount} damage from <b>{attackerName}</b>. Current Health: {CurrentHealth}/{maxHealth}");

        EventManager.TriggerEvent(EventType.PlayerHealthChanged, CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }
    private IEnumerator InvincibilityRoutine()
    {
        IsInvulnerable = true;
        OnInvincibilityStarted?.Invoke(invincibilityDuration); // Notifica inicio

        yield return new WaitForSeconds(invincibilityDuration);

        IsInvulnerable = false;
        OnInvincibilityEnded?.Invoke(); // Notifica fin
    }
    private void Die()
    {
        Debug.Log("<color=black><b>Player has DIED!</b></color>");
        OnDeath?.Invoke();
        EventManager.TriggerEvent(EventType.GameOver);
    }
    public void Heal(float amount)
    {
        if (CurrentHealth <= 0f) return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        Debug.Log($"<color=green>[Heal]</color> Player healed {amount} HP. Current Health: {CurrentHealth}/{maxHealth}");
        EventManager.TriggerEvent(EventType.PlayerHealthChanged, CurrentHealth, maxHealth);
    }
}