using UnityEngine;
public class PatrolEnemyService : EnemyService<PatrolEnemy>
{
    public PatrolEnemyService(PatrolEnemy prefab, Transform parent, int size): base(new PatrolEnemyFactory(prefab, parent), size) { }
    public PatrolEnemy Spawn(Vector2 position, float direction, float minX, float maxX)
    {
        PatrolEnemy enemy = GetFromPool();
        enemy.Initialize(position, direction, minX, maxX);
        return enemy;
    }
}
