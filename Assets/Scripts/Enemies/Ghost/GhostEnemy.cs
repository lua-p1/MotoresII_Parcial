using UnityEngine;
public class GhostEnemy : EnemyBase
{
    [Header("Horizontal Movement Config")]
    [SerializeField] private float horizontalSpeed = 3f;
    [Header("Smooth Floating (Flotación Constante)")]
    [SerializeField] private float floatAmplitude = 0.35f;
    [SerializeField] private float floatFrequency = 2.5f;
    [Header("Height Change (Cambio de Plataforma)")]
    [SerializeField] private float verticalMoveInterval = 1.8f;
    [SerializeField] private float minVerticalDistance = 1.5f;
    [SerializeField] private float maxVerticalDistance = 2.5f;
    [SerializeField] private float smoothTime = 0.7f;
    private IMovementEnemy _movement;
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX, float minY, float maxY)
    {
        transform.position = spawnPos;

        _movement = new SineMovement(transform, spawnPos, initialDirection, minX, maxX, minY, maxY,
            horizontalSpeed, floatAmplitude, floatFrequency,
            verticalMoveInterval, minVerticalDistance, maxVerticalDistance, smoothTime);
    }
    protected override void Update()
    {
        base.Update();
        _movement.Move();
    }
}