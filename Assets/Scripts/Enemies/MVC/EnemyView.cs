using System;
using System.Collections.Generic;
using UnityEngine;
//El EnemyView se encarga de todo lo relacionado con la representacion visual del enemigo
public class EnemyView : IObserver<EnemyEvent>
{
    private Transform _transform;
    private Animator _animator;
    private Dictionary<EnemyEvent, Action> _actions;
    public EnemyView(EnemyBase user)
    {
        _transform = user.transform;
        _animator = user.GetComponent<Animator>();
        _actions = new Dictionary<EnemyEvent, Action>();
        FillDictionary();
    }
    //Diccionario de acciones que responde a todas las acciones visuales del enemigo
    private void FillDictionary()
    {
        _actions.Add(EnemyEvent.Reset, ResetView);
        _actions.Add(EnemyEvent.Move, () =>
        {
            SetMoving(true);
        });
        _actions.Add(EnemyEvent.FaceLeft, () =>
        {
            Flip(-1f);
        });
        _actions.Add(EnemyEvent.FaceRight, () =>
        {
            Flip(1f);
        });
        _actions.Add(EnemyEvent.AttackPlayer, OnAttack);
        _actions.Add(EnemyEvent.EnemyDie, () =>
        {
            SetMoving(false);
            OnEnemyDeath();
        });
        _actions.Add(EnemyEvent.Pause, () =>
        {
            SetAnimatorSpeed(0f);
        });
        _actions.Add(EnemyEvent.Resume, () =>
        {
            SetAnimatorSpeed(1f);
        });
    }
    //Metodo para girar al enemigo
    protected virtual void Flip(float direction)
    {
        Vector3 scale = _transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        _transform.localScale = scale;
    }
    //Booleano para ver si se reproduce la animacion de movimiento
    private void SetMoving(bool isMoving)
    {
        if (_animator == null) return;
        _animator.SetBool("IsMoving", isMoving);
    }
    //Trigger para reproducir animacion de ataque
    protected virtual void OnAttack()
    {
        if (_animator == null) return;
        _animator.SetTrigger("OnAttack");
    }
    //Trigger para reproducir animacion de Muerte
    protected virtual void OnEnemyDeath()
    {
        if (_animator == null) return;
        _animator.SetTrigger("OnDeath");
    }
    protected virtual void ResetView()
    {
        if (_animator == null) return;
        //Restablece el Animator y vuelve a vincular sus propiedades con los componentes del GameObject.
        //En la practica, reinicia el estado de las animaciones y sus valores, como los parametros del Animator.
        //Por ejemplo, si antes el enemigo tenia activado el parametro IsMoving, al reiniciarlo ya no queremos conservar ese estado anterior.
        _animator.Rebind();
        //Fuerza al Animator a actualizarse inmediatamente.
        //El 0f significa que realiza esa actualizacion con un tiempo de avance de cero segundos.
        _animator.Update(0f);
        //Establece la velocidad de reproduccion del Animator en 1.
        _animator.speed = 1f;
    }
    //Establece la velocidad de reproduccion del Animator en el valor pasado por parametro.
    private void SetAnimatorSpeed(float speed)
    {
        if (_animator == null) return;
        _animator.speed = speed;
    }
    //Metodo que se llama cada vez que un IObservable hace el metodo
    //NotifyObservers
    public void Notify(EnemyEvent action)
    {
        if (_actions.ContainsKey(action))
            _actions[action].Invoke();
    }
}
