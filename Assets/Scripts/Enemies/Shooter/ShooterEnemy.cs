using UnityEngine;
//El Tirador ES un Patrol que ademas dispara. Hereda de PatrolEnemy, asi reutiliza
//su movimiento (PatrolMovement con su speed) y su Initialize.
//Tiene su propio pool y spawner (ShooterEnemyService y ShooterEnemySpawner), igual que Patrol y Ghost.
public class ShooterEnemy : PatrolEnemy
{
    [Header("Shooter Config")]
    [Tooltip("Distancia a la que tiene que estar el player para empezar a apuntarle")]
    [SerializeField] private float detectionRange = 7f;
    [Tooltip("Segundos que apunta (avisando en rojo) antes de disparar")]
    [SerializeField] private float aimDuration = 0.6f;
    [Tooltip("Segundos que se queda quieto despues de disparar")]
    [SerializeField] private float reloadDuration = 1.5f;
    [Tooltip("Color de aviso mientras apunta")]
    [SerializeField] private Color aimColor = Color.red;
    [Header("Projectile")]
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private float projectileDamage = 1f;
    [Tooltip("Cuantas balas se crean de entrada (si hacen falta mas, el pool crea solas)")]
    [SerializeField] private int projectilePoolSize = 3;
    private ShooterModel _shooterModel;
    private ShooterController _shooterController;
    private Transform _player;
    private ProjectileService _projectiles;

    //Antes del Awake del padre preparo lo que necesita el Controller, que se crea adentro de base.Awake:
    //el player y el pool de balas.
    protected override void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(Tags.Player);
        if (playerObject != null) _player = playerObject.transform;
        //Las balas van al mismo padre que el Tirador (el spawner, dentro de Root), no adentro del Tirador:
        //asi no se mueven con el y la pausa las alcanza.
        _projectiles = new ProjectileService(projectilePrefab, transform.parent, projectilePoolSize);
        base.Awake();
    }
    //Cada vez que el pool lo prende arranca patrullando (aunque haya muerto mientras apuntaba)
    private void OnEnable()
    {
        if (_shooterController != null) _shooterController.ResetStates();
    }
    //Si esta muerto o el juego no esta en Playing, hace lo de siempre (base.Update: espera de muerte, etc.).
    //Si esta vivo y jugando, la FSM decide que hacer (en Patrol se mueve con el Tick comun).
    protected override void Update()
    {
        if (!CanAct)
        {
            base.Update();
            return;
        }
        _shooterController.UpdateStates();
    }
    protected override EnemyModel CreateModel()
    {
        _shooterModel = new ShooterModel(isVulnerableToWeapon, damageToPlayer,
            detectionRange, aimDuration, reloadDuration, projectileSpeed, projectileDamage);
        return _shooterModel;
    }
    protected override EnemyView CreateView()
    {
        return new ShooterView(this, aimColor);
    }
    protected override EnemyController CreateController(EnemyModel model)
    {
        _shooterController = new ShooterController(_shooterModel, gameObject, _player, _projectiles);
        return _shooterController;
    }
}
