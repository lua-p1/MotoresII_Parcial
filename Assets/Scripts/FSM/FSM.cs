using System.Collections.Generic;
//Maquina de estados: guarda los estados por ID, cual es el actual, y se encarga de cambiarlo.
public class FSM
{
    //Estados posibles de la FSM. Por ahora los usa el Tirador (ShooterController)
    public enum StateID
    {
        Patrol,
        Aim,
        Reload,
    }
    private Dictionary<StateID, IState> _allStates = new Dictionary<StateID, IState>();
    private IState _currentState;
    //Guarda un estado con su ID (si ya existe uno con ese ID, no hace nada)
    public void AddState(StateID key, IState value)
    {
        if (_allStates.ContainsKey(key)) return;
        _allStates.Add(key, value);
    }
    //Sale del estado actual (si hay) y entra al nuevo
    public void ChangeState(StateID key)
    {
        if (!_allStates.ContainsKey(key)) return;
        _currentState?.OnExit();
        _currentState = _allStates[key];
        _currentState.OnEnter();
    }
    //Le delega el frame al estado actual
    public void OnUpdateState()
    {
        _currentState?.OnUpdate();
    }
}
