using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Patrolstate : State
{
    public override void OnEnter()
    {
        //Debug.Log("Entro al patrol");
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del Patrol");
    }

    public override void OnUpdate()
    {
        patrol();
        //Debug.Log("Estoy en el Patrol");
    }

    public void patrol()
    {
        
    }

}
