using UnityEngine;

public class GhostEnemy : EnemyBase
{
    [Header("Horizontal Movement Config")]
    [SerializeField] private float horizontalSpeed = 3f;

    [Header("Smooth Floating (Flotación Constante)")]
    [Tooltip("Qué tanto sube y baja constantemente (onda constante)")]
    [SerializeField] private float floatAmplitude = 0.35f;

    [Tooltip("Qué tan rápido flota/se balancea")]
    [SerializeField] private float floatFrequency = 2.5f;

    [Header("Height Change (Cambio de Plataforma)")]
    [SerializeField] private float verticalMoveInterval = 1.8f;
    [SerializeField] private float minVerticalDistance = 1.5f;
    [SerializeField] private float maxVerticalDistance = 2.5f;

    [Tooltip("Tiempo de suavizado para cambiar de altura (mayor = más suave/lento)")]
    [SerializeField] private float smoothTime = 0.7f;

    private float _direction = 1f;
    private float _minX, _maxX;
    private float _minY, _maxY;

    private float _currentBaseY;
    private float _targetBaseY;
    private float _yVelocity = 0f;
    private float _verticalTimer = 0f;
    private float _randomSeed; // Evita que múltiples fantasmas floten en perfecta sincronía
    private bool _hasEnteredScreen = false;

    /// <summary>
    /// Configura los límites y la dirección del fantasma.
    /// </summary>
    public void Initialize(Vector2 spawnPos, float initialDirection, float minX, float maxX, float minY, float maxY)
    {
        transform.position = spawnPos;
        _direction = Mathf.Sign(initialDirection);
        _minX = minX;
        _maxX = maxX;
        _minY = minY;
        _maxY = maxY;

        _currentBaseY = spawnPos.y;
        _targetBaseY = spawnPos.y;
        _verticalTimer = 0f;
        _yVelocity = 0f;
        _hasEnteredScreen = false;

        // Semilla única para desincronizar el balanceo de cada fantasma
        _randomSeed = Random.Range(0f, 100f);

        UpdateSpriteFacing();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        Move();
        HandleVerticalTimer();
    }

    private void Move()
    {
        // 1. Movimiento Horizontal (con rebote en bordes)
        float newX = transform.position.x + (_direction * horizontalSpeed * Time.deltaTime);

        if (!_hasEnteredScreen)
        {
            if (newX >= _minX && newX <= _maxX)
            {
                _hasEnteredScreen = true;
            }
        }
        else
        {
            if (newX <= _minX && _direction < 0)
            {
                _direction = 1f;
                UpdateSpriteFacing();
            }
            else if (newX >= _maxX && _direction > 0)
            {
                _direction = -1f;
                UpdateSpriteFacing();
            }
        }

        // 2. Transición Suave de la Altura Base (SmoothDamp)
        _currentBaseY = Mathf.SmoothDamp(_currentBaseY, _targetBaseY, ref _yVelocity, smoothTime);

        // 3. Balanceo de Flotación (Onda Senoidal Constante)
        float floatOffset = Mathf.Sin((Time.time + _randomSeed) * floatFrequency) * floatAmplitude;

        // Y Final delimitada estrictamente entre min/max Y
        float finalY = Mathf.Clamp(_currentBaseY + floatOffset, _minY, _maxY);

        transform.position = new Vector2(newX, finalY);
    }

    private void HandleVerticalTimer()
    {
        _verticalTimer += Time.deltaTime;
        if (_verticalTimer >= verticalMoveInterval)
        {
            _verticalTimer = 0f;
            PerformRandomVerticalMove();
        }
    }

    private void PerformRandomVerticalMove()
    {
        // Elige aleatoriamente subir (1) o bajar (-1)
        float sign = Random.value > 0.5f ? 1f : -1f;
        float distance = Random.Range(minVerticalDistance, maxVerticalDistance) * sign;

        // Define el nuevo nivel objetivo
        _targetBaseY = Mathf.Clamp(_currentBaseY + distance, _minY, _maxY);
    }

    private void UpdateSpriteFacing()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (_direction > 0 ? 1 : -1);
        transform.localScale = scale;
    }
}