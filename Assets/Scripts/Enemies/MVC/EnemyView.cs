using System;
using System.Collections.Generic;
using UnityEngine;
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
    protected virtual void Flip(float direction)
    {
        Vector3 scale = _transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        _transform.localScale = scale;
    }
    private void SetMoving(bool isMoving)
    {
        if (_animator == null) return;
        _animator.SetBool("IsMoving", isMoving);
    }
    protected virtual void OnAttack()
    {
        if (_animator == null) return;
        _animator.SetTrigger("OnAttack");
    }
    protected virtual void OnEnemyDeath()
    {
        if (_animator == null) return;
        _animator.SetTrigger("OnDeath");
    }
    protected virtual void ResetView()
    {
        if (_animator == null) return;
        _animator.Rebind();
        _animator.Update(0f);
        _animator.speed = 1f;
    }
    private void SetAnimatorSpeed(float speed)
    {
        if (_animator == null) return;
        _animator.speed = speed;
    }
    public void Notify(EnemyEvent action)
    {
        if (_actions.ContainsKey(action))
            _actions[action].Invoke();
    }
}
