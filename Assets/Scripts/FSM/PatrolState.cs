using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Patrolstate : State
{
    int currentIndex = 0;
    Node exitNode = null;
    public override void OnEnter()
    {
        //Debug.Log("Entro al patrol");
        exitNode = null;
    }

    public override void OnExit()
    {
        fsm.enemy._toPatrol = exitNode;
        //Debug.Log("Salgo del Patrol");
    }

    public override void OnUpdate()
    {
        //Debug.Log("ejecutando update de patrol");
        Node nextNode = fsm.enemy._patrolNodes[currentIndex];
        exitNode = nextNode;
        if (fsm.enemy.LineOfSight(nextNode.transform))
        { 
            goToNode(nextNode);
        }
        else
        {
            
            fsm.ChangeState(PlayerState.Reset);
        }
    }

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
}
