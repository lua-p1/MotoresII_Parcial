//Estado Patrol del Tirador: camina por su plataforma hasta que el player entra en rango.
public class ShooterPatrolState : IState
{
    private ShooterController _controller;
    public ShooterPatrolState(ShooterController controller)
    {
        _controller = controller;
    }
    //Al entrar vuelve a caminar (despues de recargar estaba quieto)
    public void OnEnter()
    {
        _controller.StartMoving();
    }
    //Camina y, si el player esta en rango, pasa a apuntar
    public void OnUpdate()
    {
        _controller.Patrol();
        if (_controller.IsPlayerInRange())
            _controller.ChangeState(FSM.StateID.Aim);
    }
    //No necesita hacer nada al salir
    public void OnExit() { }
}
