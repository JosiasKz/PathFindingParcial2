using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//Este state se encarga de patrullar. Cuando está en este estado, patrulla uno por uno los nodos que tiene en _patrolNodes
public class Patrolstate : State
{
    int currentIndex = 0;
    Node exitNode = null;
    public override void OnEnter()
    {
        //Debug.Log("Entro al patrol");
        exitNode = null;
    }

    //Cuando sale de este estado, se guarda el nodo donde habia quedado en _ToPatrol
    //Para luego retomarlo cuando vuelva a patrullar
    public override void OnExit()
    {
        fsm.enemy._toPatrol = exitNode;
        //Debug.Log("Salgo del Patrol");
    }

    public override void OnUpdate()
    {
        //Va yendo de nodo a nodo
        Node nextNode = fsm.enemy._patrolNodes[currentIndex];
        exitNode = nextNode;
        //pero si el nodo destino no está a la vista entonces cambia al state Reset
        if (fsm.enemy.LineOfSight(nextNode.transform))
        { 
            goToNode(nextNode);
        }
        else
        {
            
            fsm.ChangeState(PlayerState.Reset);
        }
    }

    //Este metodo se encarga de que el enemy se mueva y rote, porque si no rota el angulo de visión siempre queda estatico
    void goToNode(Node nodeToGo)
    {
        if (nodeToGo != null)
        {
            Vector3 dir = nodeToGo.transform.position - fsm.enemy.transform.position;
            //dir.z = 0;
            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                fsm.enemy.transform.rotation = Quaternion.Lerp(
                    fsm.enemy.transform.rotation,
                    targetRot,
                    Time.deltaTime * 8f // velocidad de rotación
                );
            }
            fsm.enemy.transform.position += dir.normalized * Time.deltaTime * fsm.enemy._speed;

            if (dir.magnitude < 0.2f)
            {
                if (currentIndex == (fsm.enemy._patrolNodes.Count - 1)) 
                {
                    currentIndex=0;
                }
                else
                {
                    currentIndex++;
                }
            }
        }
    }

    //En caso de que se ejecute el evento de deteccion del player, interrumpe este estado para cambiar al de Persuit
    public override void OnPlayerDetected(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name+" ON PLAYER DETECTED ON PATROL");
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);
        fsm.ChangeState(PlayerState.Persuit);
    }

    //En caso de que se ejecute el evento de alerta recibida, interrumpe este estado para cambiar al de Reset con destino al nodo más cercano donde se vió al player
    public override void OnAlertReceived(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON ALERT RECEIVED PERSUIT");
        fsm.ChangeState(PlayerState.Reset);
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);

    }
}
