using UnityEngine;
public class GhostEnemyFactory : Factory<GhostEnemy>
{
    public GhostEnemyFactory(GhostEnemy prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }
}

