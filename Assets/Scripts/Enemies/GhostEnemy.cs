using UnityEngine;

public class GhostEnemy : EnemyBase
{
    [Header("Ghost Movement Config")]
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float verticalAmplitude = 5f; // Amplitud del movimiento arriba/abajo
    [SerializeField] private float verticalFrequency = 0.3f;   // Velocidad de la onda arriba/abajo

    private float _direction = 1f; // 1: Izquierda a Derecha, -1: Derecha a Izquierda
    private float _targetBoundaryX;
    private float _startY;
    private float _minY;
    private float _maxY;

    /// <summary>
    /// Configura la trayectoria del fantasma al spawnear.
    /// </summary>
    public void Initialize(Vector2 spawnPosition, float direction, float targetX, float minY, float maxY)
    {
        transform.position = spawnPosition;
        _direction = Mathf.Sign(direction);
        _targetBoundaryX = targetX;
        _startY = spawnPosition.y;
        _minY = minY;
        _maxY = maxY;

        // Voltear sprite según la dirección
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (_direction > 0 ? 1 : -1);
        transform.localScale = scale;
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        Move();
        CheckBoundary();
    }

    private void Move()
    {
        // 1. Movimiento Horizontal
        float newX = transform.position.x + (_direction * speed * Time.deltaTime);

        // 2. Movimiento Vertical (Onda sinusoidal delimitada por los bordes superior e inferior)
        float wave = Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        float newY = Mathf.Clamp(_startY + wave, _minY, _maxY);

        transform.position = new Vector2(newX, newY);
    }

    private void CheckBoundary()
    {
        // Si superó el lado opuesto de la pantalla, regresa al pool
        if ((_direction > 0 && transform.position.x >= _targetBoundaryX) ||
            (_direction < 0 && transform.position.x <= _targetBoundaryX))
        {
            Despawn();
        }
    }
}