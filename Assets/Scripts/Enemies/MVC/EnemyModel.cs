using UnityEngine;

//EnemyModel es el responsable de almacenar y controlar los datos propios del enemigo.
public class EnemyModel : IObservable<EnemyEvent>
{
    private ObserverList<EnemyEvent> _enemyObservers;
    private bool _isDead;
    private float _direction;
    private bool _isVulnerableToWeapon;
    private int _damageToPlayer;
    //En el constructor genero la lista de observadores,
    //si las armas le hacen damage y cuanto damage inflige al player.
    public EnemyModel(bool isVulnerableToWeapon, int damageToPlayer)
    {
        _enemyObservers = new ObserverList<EnemyEvent>();
        _isVulnerableToWeapon = isVulnerableToWeapon;
        _damageToPlayer = damageToPlayer;
    }
    //Metodo que se llama en los EnemyBase
    //Cuando se Initialize.
    //Entre el padre y el hijo hay un metodo llamado ResetEnemy
    //que hace de intermediario.
    public void Reset()
    {
        _isDead = false;
        _direction = 0f;
        //Avisa a todos los observers, solo reacciona el EnemyView
        NotifyObservers(EnemyEvent.Reset);
    }
    //Metodo que se llama en Controller 
    public void StartMoving()
    {
        //Avisa a todos los observers, solo reacciona el EnemyView
        NotifyObservers(EnemyEvent.Move);
    }
    //sirve para enviar una notificacion al EnemyView para flipear.
    //La logica de movimientos esta en la estrategia de movimientos.
    public void SetDirection(float direction)
    {
        // Mathf.Sign convierte un número en su signo:
        // Mathf.Sign(-5) devuelve -1 (izquierda).
        // Mathf.Sign(5) devuelve 1 (derecha).
        // Así obtenemos solamente la dirección, sin importar la magnitud del valor.
        //
        // Mathf.Approximately compara la nueva dirección con la dirección anterior (_direction).
        // Si ambas son iguales, devuelve true y el método termina con return,
        // evitando notificar al EnemyView innecesariamente.
        // Si son diferentes, actualiza la direccion y notifica el cambio de orientación.
        float newDirection = Mathf.Sign(direction);
        if (Mathf.Approximately(newDirection, _direction)) return;
        _direction = newDirection;
        //Avisa a todos los observers, solo reacciona el EnemyView
        NotifyObservers(_direction > 0 ? EnemyEvent.FaceRight : EnemyEvent.FaceLeft);
    }

    //Si es invunerable o esta muerto volve, sino matalo
    public bool TryKillByWeapon()
    {
        if (_isDead || !_isVulnerableToWeapon) return false;
        Kill();
        return true;
    }
    //si esta muerto volve sino setealo muerto
    public void Kill()
    {
        if (_isDead) return;
        _isDead = true;
        //La notificacion le llega a EnemyView y a EnemyBase
        //para controlar animacion y eventos de muerte.
        NotifyObservers(EnemyEvent.EnemyDie);
    }
    //Lo llama el EnemyController (en AttackPlayer) despues de intentar hacerle damage al player
    public void RegisterAttack()
    {
        //Avisa a todos los observers, solo reacciona el EnemyView
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
