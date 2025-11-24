using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();

    public virtual void OnPlayerDetected(Vector3 playerPos) { }
    public virtual void OnAlertReceived(Vector3 playerPos) { }

    public FiniteStateMachine fsm;
}
