using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitState : State
{
    public override void OnEnter()
    {
        //Debug.Log("Entro al Hunt");
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del Hunt");
    }

    public override void OnUpdate()
    {
        EnergyCheck();
    }


    //Este metodo lo agregué porque luego de comer un boid, el cazador quedaba en estado de hunt pero sin presa por un rato
    public void backToPatrol()
    {
        fsm.ChangeState(PlayerState.Patrol);
    }

    //Si me quedo sin energia, vamos a idle para recargar
    void EnergyCheck()
    {

    }
}
