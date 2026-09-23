using UnityEngine;
public class PatrolEnemy : EnemyBase
{
    [Header("Patrol Movement Config")]
    [SerializeField] private float speed = 2f;
    private IMovementEnemy _movement;
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX)
    {
        transform.position = spawnPos;
        _movement = new PatrolMovement(transform, initialDirection, minX, maxX, speed);
    }
    private void Update()
    {
        if (!CanAct) return;
        _movement.Move();
    }
}
