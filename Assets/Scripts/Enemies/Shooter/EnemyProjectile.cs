using System;
using UnityEngine;
//Bala que dispara el Tirador. Va en linea recta en la direccion que le pasan y atraviesa las plataformas.
//- Si toca al player le hace damage y vuelve al pool.
//- Si la toca la guadania (Weapon) se destruye y vuelve al pool.
//- Si no toca nada, vuelve al pool cuando se termina su tiempo de vida.
//No se destruye nunca: la maneja un pool (ProjectileService), igual que a los enemigos.
[RequireComponent(typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour, IPauseable
{
    [Tooltip("Segundos que vive la bala antes de volver al pool si no choca con nada")]
    [SerializeField] private float lifetime = 4f;
    //Hacia donde va (normalizada)
    private Vector2 _direction;
    private float _speed;
    private float _damage;
    //Tiempo que le queda de vida
    private float _lifeTimer;
    //Lo setea el ProjectileService: es lo que hace al volver al pool
    private Action<EnemyProjectile> _returnToPoolCallback;
    private void Awake()
    {
        //Tiene que ser trigger: no choca fisicamente, solo detecta al player y al arma
        GetComponent<Collider2D>().isTrigger = true;
    }
    public void SetReturnToPoolCallback(Action<EnemyProjectile> callback)
    {
        _returnToPoolCallback = callback;
    }
    //Lo llama el ProjectileService cuando la saca del pool: la ubica y le dice hacia donde ir
    public void Launch(Vector2 position, Vector2 direction, float speed, float damage)
    {
        transform.position = position;
        _direction = direction;
        _speed = speed;
        _damage = damage;
        _lifeTimer = lifetime;
    }
    private void Update()
    {
        if (!GameManager.IsPlaying) return;
        //Avanza en linea recta: direccion * velocidad * tiempo del frame
        transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
        //Si se termina su tiempo de vida, vuelve al pool
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
            Despawn();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.IsPlaying) return;
        //La guadania (solo si esta activa) destruye la bala
        if (collision.CompareTag(Tags.Weapon) && collision.enabled)
        {
            Despawn();
        }
        //Si toca al player le hace damage (si es invulnerable, PlayerHealth lo ignora) y la bala desaparece igual
        else if (collision.CompareTag(Tags.Player))
        {
            if (collision.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damage, name);
            Despawn();
        }
    }
    //Vuelve al pool (o se apaga si no tiene pool)
    private void Despawn()
    {
        if (_returnToPoolCallback != null)
            _returnToPoolCallback(this);
        else
            gameObject.SetActive(false);
    }
    public void Pause()
    {
        enabled = false;   // frena el Update: la bala queda quieta
    }
    public void Resume()
    {
        enabled = true;
    }
}
