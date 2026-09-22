using UnityEngine;
public class PatrolEnemyFactory : Factory<PatrolEnemy>
{
    public PatrolEnemyFactory(PatrolEnemy prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }
}
