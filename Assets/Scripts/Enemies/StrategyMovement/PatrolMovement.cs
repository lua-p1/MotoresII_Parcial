using UnityEngine;
public class PatrolMovement : IMovementEnemy
{
    private Transform _target;
    private float _speed;
    private float _minX, _maxX;
    private float _direction;
    public float Direction => _direction;
    public PatrolMovement(Transform target, float initialDirection, float minX, float maxX, float speed)
    {
        _target = target;
        _direction = Mathf.Sign(initialDirection);
        _minX = minX;
        _maxX = maxX;
        _speed = speed;
    }
    public void Move()
    {
        float newX = _target.position.x + (_direction * _speed * Time.deltaTime);
        if (newX <= _minX && _direction < 0)
            _direction = 1f;
        else if (newX >= _maxX && _direction > 0)
            _direction = -1f;
        _target.position = new Vector2(Mathf.Clamp(newX, _minX, _maxX), _target.position.y);
    }
}
