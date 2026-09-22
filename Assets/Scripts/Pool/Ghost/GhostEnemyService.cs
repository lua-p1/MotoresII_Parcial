using UnityEngine;
public class GhostEnemyService : EnemyService<GhostEnemy>
{
    public GhostEnemyService(GhostEnemy prefab, Transform parent, int size): base(new GhostEnemyFactory(prefab, parent), size) { }
    public GhostEnemy Spawn(Vector2 position, float direction, float minX, float maxX, float minY, float maxY)
    {
        GhostEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX, minY, maxY);
        return enemy;
    }
}
