using UnityEngine;
public class PatrolEnemy : EnemyBase
{
    [Header("Patrol Movement Config")]
    [SerializeField] private float speed = 2f;
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX)
    {
        IMovementEnemy movement = new PatrolMovement(transform, initialDirection, minX, maxX, speed);
        ResetEnemy(spawnPos, movement);
    }
}
