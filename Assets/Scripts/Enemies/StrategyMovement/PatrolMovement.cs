using UnityEngine;
public class PatrolMovement : IMovementEnemy
{
    //El target es el propio enemigo
    private Transform _target;
    private float _speed;
    private float _minX, _maxX;
    //Direccion a la que se mueve
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
    //Metodo de la estrategia de movimientos del Patrol
    //Se mueve solo en X: si llega al maximo o al minimo cambia de direccion, y en Y siempre se mantiene
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
