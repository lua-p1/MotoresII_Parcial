using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Combat Config")]
    [Tooltip("Can die if touch weapon?")]
    public bool isVulnerableToWeapon = true;
    [Tooltip("Attack dmg if touch player")]
    public int damageToPlayer = 1;
    protected bool hasTouchedPlayer = false;
    protected bool isDead = false;
    protected Collider2D enemyCollider;
    protected virtual void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        enemyCollider.isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        hasTouchedPlayer = false;
        if (enemyCollider != null) enemyCollider.enabled = true;
    }

    protected virtual void Update()
    {
        if (isDead) return;
    }

    protected virtual void DamagePlayer(GameObject playerObject)
    {
        // Intenta obtener cualquier componente en el Player que implemente IDamageable
        if (playerObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damageToPlayer, gameObject.name);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Weapon") && collision.enabled)
        {
            if (isVulnerableToWeapon)
            {
                Die();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            if (!hasTouchedPlayer)
            {
                hasTouchedPlayer = true;
                DamagePlayer(collision.gameObject); // Pasa la referencia del GameObject del Player
            }
        }
    }
    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // Notificar al sistema que un enemigo fue derrotado
        EventManager.OnEnemyKilled?.Invoke();

        Despawn();
    }

    public virtual void Despawn()
    {
        gameObject.SetActive(false);
    }
}