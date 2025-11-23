using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class IdleState : State
{
    public override void OnEnter()
    {
        fsm.hunter.resting = true;
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del idle");
    }

    //el cazador para de moverse y recarga energias hasta que esté full
    public override void OnUpdate()
    {

    }

}
