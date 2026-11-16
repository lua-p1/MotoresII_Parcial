using UnityEngine;
//Servicio de balas: vincula el pool con el factory, igual que EnemyService con los enemigos.
//Al crear el servicio, el pool ya instancia las balas y las deja apagadas.
//Cuando el Tirador dispara, saca una bala del pool y la lanza; cuando la bala termina, vuelve sola al pool.
public class ProjectileService
{
    private Pool<EnemyProjectile> _pool;
    private Factory<EnemyProjectile> _factory;
    public ProjectileService(EnemyProjectile prefab, Transform parent, int size)
    {
        _factory = new ProjectileFactory(prefab, parent);
        _pool = new Pool<EnemyProjectile>(CreateProjectile, TurnOn, TurnOff, size);
    }
    //Lo usa el pool cuando necesita una bala nueva: la crea con el factory y le dice como volver al pool
    private EnemyProjectile CreateProjectile()
    {
        EnemyProjectile projectile = _factory.CreateObject();
        projectile.SetReturnToPoolCallback(ReturnToPool);
        return projectile;
    }
    //Lo llama el ShooterController al disparar: saca una bala del pool y la lanza
    public void Spawn(Vector2 position, Vector2 direction, float speed, float damage)
    {
        EnemyProjectile projectile = _pool.GetObject();
        projectile.Launch(position, direction, speed, damage);
    }
    private void TurnOn(EnemyProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }
    private void TurnOff(EnemyProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
    private void ReturnToPool(EnemyProjectile projectile)
    {
        _pool.ReturnObject(projectile);
    }
}
