//Model del Tirador. Hereda de EnemyModel, asi tiene todo lo de cualquier enemigo
//(vida, direccion, damage, lista de observers) y le suma lo propio del disparo:
//- los datos del disparo (rango, tiempos, velocidad y damage de la bala)
//- los avisos de que apunta (Aim) y de que disparo (Shoot)
public class ShooterModel : EnemyModel
{
    //Distancia a la que tiene que estar el player para que empiece a apuntar
    private float _detectionRange;
    //Cuanto tiempo apunta (avisando) antes de disparar
    private float _aimDuration;
    //Cuanto tiempo se queda quieto despues de disparar
    private float _reloadDuration;
    //Velocidad de la bala
    private float _projectileSpeed;
    //Damage que hace la bala al player
    private float _projectileDamage;
    public ShooterModel(bool isVulnerableToWeapon, int damageToPlayer,
        float detectionRange, float aimDuration, float reloadDuration,
        float projectileSpeed, float projectileDamage)
        : base(isVulnerableToWeapon, damageToPlayer)
    {
        _detectionRange = detectionRange;
        _aimDuration = aimDuration;
        _reloadDuration = reloadDuration;
        _projectileSpeed = projectileSpeed;
        _projectileDamage = projectileDamage;
    }
    //Lo llama el ShooterController cuando entra al estado Aim (se frena y apunta)
    public void StartAiming()
    {
        //Avisa a todos los observers, solo reacciona el ShooterView
        NotifyObservers(EnemyEvent.Aim);
    }
    //Lo llama el ShooterController cuando dispara
    public void RegisterShot()
    {
        //Avisa a todos los observers, solo reacciona el ShooterView
        NotifyObservers(EnemyEvent.Shoot);
    }
    public float DetectionRange => _detectionRange;
    public float AimDuration => _aimDuration;
    public float ReloadDuration => _reloadDuration;
    public float ProjectileSpeed => _projectileSpeed;
    public float ProjectileDamage => _projectileDamage;
}
