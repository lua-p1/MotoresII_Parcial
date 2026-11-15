using UnityEngine;

public class EnemyController
{
    private EnemyModel _model;
    private IMovementEnemy _movement;
    private GameObject _owner;
    public EnemyController(EnemyModel model, GameObject owner)
    {
        _model = model;
        _owner = owner;
    }
    //Lo llama el EnemyBase cuando el pool entrega el enemigo
    public void SetMovement(IMovementEnemy movement)
    {
        _movement = movement;
        _model.SetDirection(_movement.Direction); //orientacion inicial antes del primer frame
        _model.StartMoving();
    }
    //Lo llama el EnemyBase en cada Update
    public void Tick()
    {
        if (_model.IsDead || _movement == null) return;
        _movement.Move();
        _model.SetDirection(_movement.Direction);
    }
    //Lo llama el EnemyBase desde OnTriggerEnter2D
    public void HandleTrigger(Collider2D collision)
    {
        if (_model.IsDead) return;
        if (collision.CompareTag("Weapon") && collision.enabled)
        {
            _model.TryKillByWeapon();
        }
        else if (collision.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject);
        }
    }
    private void AttackPlayer(GameObject playerObject)
    {
        if (!playerObject.TryGetComponent<IDamageable>(out var damageable)) return;
        damageable.TakeDamage(_model.DamageToPlayer, _owner.name);
        _model.RegisterAttack();
    }
}
