using UnityEngine;
//Estado Aim del Tirador: se frena, mira al player y avisa (se pone rojo).
//Cuando termina el tiempo de apuntar, dispara y pasa a Reload.
public class ShooterAimState : IState
{
    private ShooterController _controller;
    private float _aimDuration;
    private float _timer;
    public ShooterAimState(ShooterController controller, float aimDuration)
    {
        _controller = controller;
        _aimDuration = aimDuration;
    }
    //Al entrar arranca el timer y avisa que apunta
    public void OnEnter()
    {
        _timer = _aimDuration;
        _controller.StartAiming();
    }
    //Sigue mirando al player mientras cuenta el tiempo; cuando llega a 0 dispara y pasa a recargar
    public void OnUpdate()
    {
        _controller.FacePlayer();
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;
        _controller.Shoot();
        _controller.ChangeState(FSM.StateID.Reload);
    }
    //No necesita hacer nada al salir
    public void OnExit() { }
}