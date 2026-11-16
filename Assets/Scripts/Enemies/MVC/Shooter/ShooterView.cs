using System;
using System.Collections.Generic;
using UnityEngine;
//View del Tirador. Hereda de EnemyView, asi ya tiene todo lo visual de cualquier enemigo
//(flip, caminar, morir, pausa, reset) y le suma lo propio del Tirador:
//- Aim: se frena y cambia de color para avisar que va a disparar.
//- Shoot: vuelve a su color normal.
public class ShooterView : EnemyView
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    //Color con el que arranca el sprite (el del prefab)
    private Color _originalColor;
    //Color de aviso mientras apunta (se configura en el Inspector del ShooterEnemy)
    private Color _aimColor;
    //Diccionario solo con los eventos propios del Tirador
    private Dictionary<EnemyEvent, Action> _shooterActions;
    //Primero corre el constructor de EnemyView (base), que arma todo lo comun.
    //Despues aca guardo lo que necesita el Tirador y lleno su diccionario.
    public ShooterView(EnemyBase user, Color aimColor) : base(user)
    {
        _animator = user.GetComponent<Animator>();
        _spriteRenderer = user.GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null) _originalColor = _spriteRenderer.color;
        _aimColor = aimColor;
        _shooterActions = new Dictionary<EnemyEvent, Action>();
        FillShooterDictionary();
    }
    private void FillShooterDictionary()
    {
        _shooterActions.Add(EnemyEvent.Aim, () =>
        {
            SetColor(_aimColor);
            SetMoving(false);
            SetTrigger("OnAim");
        });
        _shooterActions.Add(EnemyEvent.Shoot, () =>
        {
            SetColor(_originalColor);
            SetTrigger("OnShoot");
        });
    }
    //Si el evento es del Tirador lo resuelvo aca. Si no (Move, flip, muerte, pausa...),
    //se lo paso al EnemyView con base.Notify para que haga lo de siempre.
    public override void Notify(EnemyEvent action)
    {
        if (_shooterActions.ContainsKey(action))
            _shooterActions[action].Invoke();
        else
            base.Notify(action);
    }
    //Cuando el pool lo reutiliza: hace el reset comun (Animator) y ademas vuelve al color original,
    //por si murio mientras apuntaba y quedo con el color de aviso.
    protected override void ResetView()
    {
        base.ResetView();
        SetColor(_originalColor);
    }
    //Cambia el color del sprite (aviso antes de disparar)
    private void SetColor(Color color)
    {
        if (_spriteRenderer == null) return;
        _spriteRenderer.color = color;
    }
    //Frena la animacion de caminar mientras apunta (cuando vuelve a patrullar, el EnemyView la prende con Move)
    private void SetMoving(bool isMoving)
    {
        if (_animator == null) return;
        _animator.SetBool("IsMoving", isMoving);
    }
    //Activa un trigger del Animator
    private void SetTrigger(string triggerName)
    {
        if (_animator == null) return;
        _animator.SetTrigger(triggerName);
    }
}
