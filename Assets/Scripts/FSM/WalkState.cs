using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State
{
    
    public override void OnEnter()
    {        
        Debug.Log("Entro al walk");
    }

    public override void OnExit()
    {
        Debug.Log("Salgo del walk");
    }

    public override void OnUpdate()
    {
        Debug.Log("Estoy en el wlak");
    }
}
