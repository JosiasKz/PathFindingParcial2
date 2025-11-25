using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    Dictionary<PlayerState, State> _allStates = new Dictionary<PlayerState, State>();
    public State _currentState;
    public Enemy enemy;
    public PlayerState currentPS;

    //Seteamos este constructor para que el fsm tenga una referencia al enemy, de esta manera poder chequear sus variables y decidir la transición de los estados
    //De paso, el fsm se suscribe a los eventos
    public FiniteStateMachine(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.OnPlayerDetected += HandlePlayerDetected;
        enemy.OnAlertReceived += HandleAlertReceived;
    }
    public void Update()
    {
        //Debug.Log(_currentState.ToString());
        _currentState?.OnUpdate();
    }
    void HandlePlayerDetected(Enemy en, Vector3 pos)
    {
        _currentState?.OnPlayerDetected(pos);

    }
    void HandleAlertReceived(Enemy en, Vector3 pos)
    {
        _currentState?.OnAlertReceived(pos);

    }
    public void AddState(PlayerState name, State state)
    {
        if (!_allStates.ContainsKey(name))
        {
            _allStates.Add(name, state);
            state.fsm = this;
        }
        else
        {
            _allStates[name] = state;
        }
    }

    public void ChangeState(PlayerState name)
    {
        //if (currentPS == name) return;
        _currentState?.OnExit();
        if(_allStates.ContainsKey(name))_currentState = _allStates[name];
        _currentState.OnEnter();
        currentPS = name;
        enemy.stateText.text=currentPS.ToString();
    }
}

public enum PlayerState
{
    Pathfinding, Patrol, Persuit, Reset
}