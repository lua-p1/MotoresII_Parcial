public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsInvulnerable { get; }
    /// <summary>
    /// Aplica damage al objeto indicando la cantidad y el nombre de la fuente/enemigo.
    /// </summary>
    void TakeDamage(float amount, string attackerName = "Unknown");
}
