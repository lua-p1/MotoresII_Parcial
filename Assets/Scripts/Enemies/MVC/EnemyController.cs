using UnityEngine;
//Se encarga de coordinar las acciones del enemigo. Es el intermediario entre el EnemyBase, el EnemyModel y el sistema de movimiento.
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
    //lo usa el EnemyBase ResetEnemy para setear la estrategia de movimiento
    //En el EnemyBase Hijo en el metodo Initialize
    public void SetMovement(IMovementEnemy movement)
    {
        _movement = movement;
        //orientacion inicial antes del primer frame
        _model.SetDirection(_movement.Direction);
        _model.StartMoving();
    }

    //Lo llama el EnemyBase en cada Update
    public void Tick()
    {
        //Si esta muerto o no tiene estrategia de movimiento volve
        if (_model.IsDead || _movement == null) return;
        //sino usa el metodo Move de la estrategia de movimiento -> IMovementEnemy
        _movement.Move();
        //Le paso la direccion al Model: si cambio, notifica y la View flipea
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
    //Metodo para atacar al player.
    private void AttackPlayer(GameObject playerObject)
    {
        if (!playerObject.TryGetComponent<IDamageable>(out var damageable)) return;
        damageable.TakeDamage(_model.DamageToPlayer, _owner.name);
        _model.RegisterAttack();
    }
}
