using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Patrolstate : State
{
    int currentIndex = 0;

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
        Debug.Log("ejecutando update de patrol");
        Node nextNode = fsm.enemy._patrolNodes[currentIndex];

        if (fsm.enemy.LineOfSight(nextNode.transform))
        {
            goToNode(nextNode);
        }
    }

    void goToNode(Node nodeToGo)
    {
        if (nodeToGo != null)
        {
            Vector3 dir = nodeToGo.transform.position - fsm.enemy.transform.position;
            //dir.z = 0;
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
