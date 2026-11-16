using UnityEngine;
//Fabrica de Tiradores: guarda el prefab y el parent, y el padre (Factory) se encarga de instanciar.
public class ShooterEnemyFactory : Factory<ShooterEnemy>
{
    public ShooterEnemyFactory(ShooterEnemy prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }
}
