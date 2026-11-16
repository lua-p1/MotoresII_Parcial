using UnityEngine;
//Fabrica de balas: guarda el prefab y el parent, y el padre (Factory) se encarga de instanciar.
public class ProjectileFactory : Factory<EnemyProjectile>
{
    public ProjectileFactory(EnemyProjectile prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }
}
