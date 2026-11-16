using System;
using System.Collections.Generic;
using UnityEngine;

//El EnemyBase se encarga de coordinar el funcionamiento general del enemigo y gestionar su ciclo de vida.
//Es el componente principal que conecta al EnemyModel, EnemyView y EnemyController, ademas de comunicarse con Unity y el Pool.
//La clase padre de todos los enemigos.
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
    private bool _isWaitingForDeathAnimation;
    private float _deathTimer;
    protected bool CanAct => !_model.IsDead && GameManager.IsPlaying;

    //En el Awake creo un model , un view, y un controller
    //El controller pide de parametro un model asi que le paso el que cree
    //Ademas como va a observar al model le hago un diccionario con acciones.
    //por ultimo hago que este script y el script EnemyView se suscriban al model.
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
    //El EnemyBase ya esta suscripto al Model (en el Awake), asi que recibe todos los eventos.
    //En el diccionario solo pongo EnemyDie, que es el unico al que reacciona, y llama a OnEnemyDeath.
    private void FillDictionary()
    {
        _actions.Add(EnemyEvent.EnemyDie, OnEnemyDeath);
    }
    //Creo un EnemyModel, si creo otro enemigo con un model disinto el base me sigo sirviendo
    //Haciendo override a este metodo
    protected virtual EnemyModel CreateModel()
    {
        return new EnemyModel(isVulnerableToWeapon, damageToPlayer);
    }
    //Creo un EnemyView, si creo otro enemigo con una view distinta el base me sigue sirviendo
    //Haciendo override a este metodo
    protected virtual EnemyView CreateView()
    {
        return new EnemyView(this);
    }
    //Creo un EnemyController, si creo otro enemigo con un controller distinto el base me sigue sirviendo
    //Haciendo override a este metodo
    protected virtual EnemyController CreateController(EnemyModel model)
    {
        return new EnemyController(model, gameObject);
    }
    //Se llama en los hijos cuando se Inicializan en el metodo Initialize
    //Luego de salir del pool en EnemyService.
    protected void ResetEnemy(Vector2 spawnPos, IMovementEnemy movement)
    {
        transform.position = spawnPos;
        _isWaitingForDeathAnimation = false;
        _deathTimer = 0f;
        _enemyCollider.enabled = true;
        _model.Reset();
        _controller.SetMovement(movement);
    }
    protected virtual void Update()
    {
        //Si esta esperando que termine la animacion de muerte llama a TickDeath
        //(con deathAnimationDuration en 0 nunca entra aca, despawnea directo)
        if (_isWaitingForDeathAnimation)
        {
            TickDeath();
            return;
        }
        //Si no puede actuar (esta muerto o el juego no esta en Playing) volve
        if (!CanAct) return;
        //Llama al tick del controller.
        _controller.Tick();
    }
    //Espero unos segundos a la animacion de muerte y despwneo
    private void TickDeath()
    {
        if (!GameManager.IsPlaying) return;
        _deathTimer -= Time.deltaTime;
        if (_deathTimer <= 0f)
            Despawn();
    }

    //Si choco contra un weapon o en un player llamo al meotod HandleTrigger
    //para saber que hacer
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CanAct) return;
        _controller.HandleTrigger(collision);
    }

    // Se llama cuando muere el enemigo
    //Desactiva el collider y avisa al los scripts en general que murio
    protected virtual void OnEnemyDeath()
    {
        _enemyCollider.enabled = false;
        EventManager.TriggerEvent(EventType.EnemyKilled);
        //Si no tiene animacion de muerte o tiempo que esperar direcamente despwnea
        if (deathAnimationDuration <= 0f)
        {
            Despawn();
            return;
        }
        // Sino espero que muera hasta activar el despawn en TickDeath
        _isWaitingForDeathAnimation = true;
        _deathTimer = deathAnimationDuration;
    }

    //Accion que hace cuando vuelve al pool que lo desactiva.
    //y lo devuelve al pool seteaado en  EnemyService que vincula el factory con el pool
    public void SetReturnToPoolCallback(Action<EnemyBase> callback)
    {
        _returnToPoolCallback = callback;
    }

    //que pasa cuando despawnea
    //Lo devuelvo al pool y marco que termino la espera de la animacion de muerte.
    //Sigue muerto en el Model hasta que vuelve a salir del pool y ResetEnemy llama a Model.Reset()
    public virtual void Despawn()
    {
        _isWaitingForDeathAnimation = false;
        if (_returnToPoolCallback != null)
            _returnToPoolCallback(this);
        else
            gameObject.SetActive(false);
    }


    #region//Metodo para suscribir Observers al Model.
    public void Subscribe(IObserver<EnemyEvent> observer)
    {
        _model.Subscribe(observer);
    }
    public void Unsubscribe(IObserver<EnemyEvent> observer)
    {
        _model.Unsubscribe(observer);
    }
    #endregion


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
    //Recibe todos los eventos que notifica el Model y ejecuta la accion solo si el evento esta en su diccionario
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