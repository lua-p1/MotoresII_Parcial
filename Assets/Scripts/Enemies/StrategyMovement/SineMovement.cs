using UnityEngine;
public class SineMovement : IMovementEnemy
{
    private readonly Transform _target;
    private readonly float _horizontalSpeed;
    private readonly float _floatAmplitude;
    private readonly float _floatFrequency;
    private readonly float _verticalMoveInterval;
    private readonly float _minVerticalDistance;
    private readonly float _maxVerticalDistance;
    private readonly float _smoothTime;
    private readonly float _minX, _maxX, _minY, _maxY;
    private float _direction;
    private float _currentBaseY;
    private float _targetBaseY;
    private float _yVelocity;
    private float _verticalTimer;
    private float _randomSeed;
    public SineMovement(Transform target, Vector2 spawnPos, float initialDirection,
        float minX, float maxX, float minY, float maxY,
        float horizontalSpeed, float floatAmplitude, float floatFrequency,
        float verticalMoveInterval, float minVerticalDistance, float maxVerticalDistance, float smoothTime)
    {
        _target = target;
        _direction = Mathf.Sign(initialDirection);
        _minX = minX; _maxX = maxX; _minY = minY; _maxY = maxY;
        _horizontalSpeed = horizontalSpeed;
        _floatAmplitude = floatAmplitude;
        _floatFrequency = floatFrequency;
        //cuada cuanto cambia la altura
        _verticalMoveInterval = verticalMoveInterval;
        _minVerticalDistance = minVerticalDistance;
        _maxVerticalDistance = maxVerticalDistance;
        _smoothTime = smoothTime;
        _currentBaseY = spawnPos.y;
        _targetBaseY = spawnPos.y;
        _randomSeed = Random.Range(0f, 100f);
        UpdateSpriteFacing();
    }
    public void Move()
    {
        MoveHorizontalAndFloat();
        HandleVerticalTimer();
    }
    //metodo de movimiento
    private void MoveHorizontalAndFloat()
    {
        //nueva poiscion en X
        float newX = _target.position.x + (_direction * _horizontalSpeed * Time.deltaTime);
        //Si llega al limite izquierdo, gira hacia la derecha.
        if (newX <= _minX && _direction < 0)
        {
            _direction = 1f;
            UpdateSpriteFacing();
            Debug.Log("Giro a la derecha");
        }
        //Si llega al limite derecho, gira hacia la izquierda.
        else if (newX >= _maxX && _direction > 0)
        {
            _direction = -1f;
            UpdateSpriteFacing();
            Debug.Log("Giro a la izquierda");
        }
        //me muevo hacia la altura target
        _currentBaseY = Mathf.SmoothDamp(_currentBaseY, _targetBaseY, ref _yVelocity, _smoothTime);
        // Recorro la onda seno usando el tiempo, sumo una semilla para que no todas sean iguales,
        // multiplico por la frecuencia para controlar que tan rapido recorre la onda
        // y por ultimo multiplico por la amplitud para controlar que tan grande es el movimiento
        float floatOffset = Mathf.Sin((Time.time + _randomSeed) * _floatFrequency) * _floatAmplitude;
        //me aseguro de no salir de los limites de Y al sumar el movimiento de la onda seno.
        float finalY = Mathf.Clamp(_currentBaseY + floatOffset, _minY, _maxY);
        //aplico la nueva posicion del enemigo.
        _target.position = new Vector2(newX, finalY);
    }

    //timer para cambiar la altura del enemigo
    private void HandleVerticalTimer()
    {
        _verticalTimer += Time.deltaTime;
        if (_verticalTimer >= _verticalMoveInterval)
        {
            _verticalTimer = 0f;
            PerformRandomVerticalMove();
        }
    }

    //metodo para cambiar la altura base del enemigo. 
    private void PerformRandomVerticalMove()
    {
        float sign = Random.value > 0.5f ? 1f : -1f;
        float distance = Random.Range(_minVerticalDistance, _maxVerticalDistance) * sign;
        _targetBaseY = Mathf.Clamp(_currentBaseY + distance, _minY, _maxY);
    }

    //metodo que actualiza hacia donde mira el enemigo
    //Hacemos un flip horizontal porque invertimos la escala en X.
    private void UpdateSpriteFacing()
    {
        Vector3 scale = _target.localScale;
        //si _direction es > 0 mira a la derecha sino a la izquirda.
        scale.x = Mathf.Abs(scale.x) * (_direction > 0 ? 1 : -1);
        _target.localScale = scale;
    }
}
