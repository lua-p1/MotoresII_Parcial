using UnityEngine;
//Estado Reload del Tirador: despues de disparar se queda quieto mirando al player.
//Es el momento para atacarlo. Cuando termina el tiempo, vuelve a patrullar.
public class ShooterReloadState : IState
{
    private ShooterController _controller;
    private float _reloadDuration;
    private float _timer;
    public ShooterReloadState(ShooterController controller, float reloadDuration)
    {
        _controller = controller;
        _reloadDuration = reloadDuration;
    }
    //Al entrar arranca el timer
    public void OnEnter()
    {
        _timer = _reloadDuration;
    }
    //Sigue mirando al player mientras cuenta el tiempo; cuando llega a 0 vuelve a patrullar
    public void OnUpdate()
    {
        _controller.FacePlayer();
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;
        _controller.ChangeState(FSM.StateID.Patrol);
    }
    //No necesita hacer nada al salir
    public void OnExit() { }
}
