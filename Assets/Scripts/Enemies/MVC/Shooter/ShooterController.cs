using UnityEngine;
//Controller del Tirador. Hereda de EnemyController, asi reutiliza todo lo de cualquier enemigo
//sin modificarlo: Tick (moverse con la estrategia) y HandleTrigger (arma y player).
//Le suma una maquina de estados (FSM) para decidir que hace en cada momento:
//  Patrol (camina) -> Aim (se frena y apunta) -> Reload (dispara y espera quieto) -> Patrol
//El ShooterEnemy llama a UpdateStates en su Update (en lugar del Tick comun).
//Los estados no hacen nada por su cuenta: le piden a este controller que patrulle, apunte o dispare.
public class ShooterController : EnemyController
{
    //El mismo Model que recibe el padre, pero con su tipo real para usar lo propio del Tirador
    private ShooterModel _shooterModel;
    //Transform del Tirador (para saber donde esta)
    private Transform _shooterTransform;
    private Transform _player;
    private ProjectileService _projectiles;
    private FSM _fsm;
    public ShooterController(ShooterModel model, GameObject owner, Transform player, ProjectileService projectiles)
        : base(model, owner)
    {
        _shooterModel = model;
        _shooterTransform = owner.transform;
        _player = player;
        _projectiles = projectiles;
        //Creo la FSM y le cargo los 3 estados (cada uno recibe este controller para pedirle cosas)
        _fsm = new FSM();
        _fsm.AddState(FSM.StateID.Patrol, new ShooterPatrolState(this));
        _fsm.AddState(FSM.StateID.Aim, new ShooterAimState(this, model.AimDuration));
        _fsm.AddState(FSM.StateID.Reload, new ShooterReloadState(this, model.ReloadDuration));
    }
    //Lo llama el ShooterEnemy en cada frame: el estado actual decide que hacer
    public void UpdateStates()
    {
        if (_shooterModel.IsDead) return;
        _fsm.OnUpdateState();
    }
    //Lo llama el ShooterEnemy cada vez que el pool lo prende: siempre arranca patrullando
    //(aunque haya muerto mientras apuntaba)
    public void ResetStates()
    {
        ChangeState(FSM.StateID.Patrol);
    }
    //Lo usan los estados para pasar de uno a otro
    public void ChangeState(FSM.StateID state)
    {
        _fsm.ChangeState(state);
    }
    //Camina reutilizando el Tick del EnemyController (mueve con la estrategia y orienta). Lo usa el estado Patrol
    public void Patrol()
    {
        Tick();
    }
    //Avisa que vuelve a caminar (lo usa el estado Patrol al entrar)
    public void StartMoving()
    {
        _shooterModel.StartMoving();
    }
    //Avisa que empieza a apuntar (lo usa el estado Aim al entrar): la ShooterView lo frena y lo pone rojo
    public void StartAiming()
    {
        _shooterModel.StartAiming();
    }
    //True si el player esta a menos de DetectionRange (las balas atraviesan todo, asi que no importa el piso)
    public bool IsPlayerInRange()
    {
        if (_player == null) return false;
        return Vector2.Distance(_shooterTransform.position, _player.position) <= _shooterModel.DetectionRange;
    }
    //Gira hacia el player: el Model avisa el cambio de direccion y la View hace el flip
    public void FacePlayer()
    {
        if (_player == null) return;
        _shooterModel.SetDirection(_player.position.x - _shooterTransform.position.x);
    }
    //Dispara una bala hacia donde esta el player en este momento
    public void Shoot()
    {
        if (_player == null) return;
        Vector2 origin = _shooterTransform.position;
        //Direccion normalizada: solo importa hacia donde, la velocidad la pone la bala
        Vector2 direction = ((Vector2)_player.position - origin).normalized;
        _projectiles.Spawn(origin, direction, _shooterModel.ProjectileSpeed, _shooterModel.ProjectileDamage);
        //Avisa que disparo (la ShooterView vuelve a su color)
        _shooterModel.RegisterShot();
    }
}
