using UnityEngine;
public class EnemyModel : IObservable<EnemyEvent>
{
    private ObserverList<EnemyEvent> _enemyObservers;
    private bool _isDead;
    private float _direction;
    private bool _isVulnerableToWeapon;
    private int _damageToPlayer;
    public EnemyModel(bool isVulnerableToWeapon, int damageToPlayer)
    {
        _enemyObservers = new ObserverList<EnemyEvent>();
        _isVulnerableToWeapon = isVulnerableToWeapon;
        _damageToPlayer = damageToPlayer;
    }

    public void Reset()
    {
        _isDead = false;
        _direction = 0f; 
        NotifyObservers(EnemyEvent.Reset);
    }
    public void StartMoving()
    {
        NotifyObservers(EnemyEvent.Move);
    }
    public void SetDirection(float direction)
    {
        float newDirection = Mathf.Sign(direction);
        if (Mathf.Approximately(newDirection, _direction)) return;
        _direction = newDirection;
        NotifyObservers(_direction > 0 ? EnemyEvent.FaceRight : EnemyEvent.FaceLeft);
    }
    public bool TryKillByWeapon()
    {
        if (_isDead || !_isVulnerableToWeapon) return false;
        Kill();
        return true;
    }
    public void Kill()
    {
        if (_isDead) return;
        _isDead = true;
        NotifyObservers(EnemyEvent.EnemyDie);
    }
    public void RegisterAttack()
    {
        NotifyObservers(EnemyEvent.AttackPlayer);
    }
    public void Subscribe(IObserver<EnemyEvent> observer)
    {
        _enemyObservers.Subscribe(observer);
    }
    public void Unsubscribe(IObserver<EnemyEvent> observer)
    {
        _enemyObservers.Unsubscribe(observer);
    }
    public void NotifyObservers(EnemyEvent action)
    {
        _enemyObservers.NotifyObservers(action);
    }
    public bool IsDead => _isDead;
    public float Direction => _direction;
    public int DamageToPlayer => _damageToPlayer;
}
