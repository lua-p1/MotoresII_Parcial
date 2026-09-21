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
        // Garantiza que sea Trigger para no colisionar físicamente con el Player
        enemyCollider.isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        // Reiniciar estado al salir del Object Pool
        isDead = false;
        hasTouchedPlayer = false;
        if (enemyCollider != null) enemyCollider.enabled = true;
    }

    protected virtual void Update()
    {
        if (isDead) return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Weapon"))
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
                DamagePlayer();
            }
        }
    }

    protected virtual void DamagePlayer()
    {
        Debug.Log($"{gameObject.name} touched player, dealt {damageToPlayer} damage.");
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        Despawn();
    }

    /// <summary>
    /// Desactiva el objeto para devolverlo al Pool en lugar de destruir la instancia.
    /// </summary>
    public virtual void Despawn()
    {
        gameObject.SetActive(false);
    }
}