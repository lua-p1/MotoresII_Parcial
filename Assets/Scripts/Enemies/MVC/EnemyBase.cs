using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour, IPauseable, IObserver<EnemyEvent>
{
    [Header("Combat Config")]
    [Tooltip("Can die if touch weapon?")]
    [SerializeField] private bool isVulnerableToWeapon = true;
    [Tooltip("Attack dmg if touch player")]
    [SerializeField] private int damageToPlayer = 1;
    [Header("Death")]
    [Tooltip("Segundos antes de volver al pool (para la animacion de muerte). 0 = al instante")]
    [SerializeField] private float deathAnimationDuration = 0f;
    private EnemyModel _model;
    private EnemyView _view;
    private EnemyController _controller;
    private Collider2D _enemyCollider;
    private Action<EnemyBase> _returnToPoolCallback;
    private Dictionary<EnemyEvent, Action> _actions;
    private bool _isDying;
    private float _deathTimer;
    protected bool CanAct => !_model.IsDead && GameManager.IsPlaying;
    protected virtual void Awake()
    {
        _enemyCollider = GetComponent<Collider2D>();
        _enemyCollider.isTrigger = true;
        _model = CreateModel();
        _view = CreateView();
        _controller = CreateController(_model);
        _actions = new Dictionary<EnemyEvent, Action>();
        FillDictionary();
        _model.Subscribe(this);
        _model.Subscribe(_view);
    }
    private void FillDictionary()
    {
        _actions.Add(EnemyEvent.EnemyDie, OnEnemyDeath);
    }
    protected virtual EnemyModel CreateModel()
    {
        return new EnemyModel(isVulnerableToWeapon, damageToPlayer);
    }
    protected virtual EnemyView CreateView()
    {
        return new EnemyView(this);
    }
    protected virtual EnemyController CreateController(EnemyModel model)
    {
        return new EnemyController(model, gameObject);
    }
    protected void ResetEnemy(Vector2 spawnPos, IMovementEnemy movement)
    {
        transform.position = spawnPos;
        _isDying = false;
        _deathTimer = 0f;
        _enemyCollider.enabled = true;
        _model.Reset();
        _controller.SetMovement(movement);
    }
    protected virtual void Update()
    {
        if (_isDying)
        {
            TickDeath();
            return;
        }
        if (!CanAct) return;
        _controller.Tick();
    }
    private void TickDeath()
    {
        if (!GameManager.IsPlaying) return;
        _deathTimer -= Time.deltaTime;
        if (_deathTimer <= 0f)
            Despawn();
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanAct) return;
        _controller.HandleTrigger(collision);
    }
    public virtual void Die()
    {
        _model.Kill();
    }
    protected virtual void OnEnemyDeath()
    {
        _enemyCollider.enabled = false;
        EventManager.TriggerEvent(EventType.EnemyKilled);
        if (deathAnimationDuration <= 0f)
        {
            Despawn();
            return;
        }
        _isDying = true;
        _deathTimer = deathAnimationDuration;
    }
    public void SetReturnToPoolCallback(Action<EnemyBase> callback)
    {
        _returnToPoolCallback = callback;
    }
    public virtual void Despawn()
    {
        _isDying = false;
        if (_returnToPoolCallback != null)
            _returnToPoolCallback(this);
        else
            gameObject.SetActive(false);
    }
    public void Subscribe(IObserver<EnemyEvent> observer)
    {
        _model.Subscribe(observer);
    }
    public void Unsubscribe(IObserver<EnemyEvent> observer)
    {
        _model.Unsubscribe(observer);
    }
    public virtual void Pause()
    {
        enabled = false;
        _model.NotifyObservers(EnemyEvent.Pause);
    }
    public virtual void Resume()
    {
        enabled = true;
        _model.NotifyObservers(EnemyEvent.Resume);
    }
    public void Notify(EnemyEvent action)
    {
        if (_actions.ContainsKey(action))
            _actions[action].Invoke();
    }
    protected virtual void OnDestroy()
    {
        if (_model == null) return;
        _model.Unsubscribe(_view);
        _model.Unsubscribe(this);
    }
}