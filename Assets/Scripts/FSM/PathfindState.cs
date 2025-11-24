using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PathfindState : State
{
    List<Node> _path = new List<Node>();
    Node _startNode;
    bool _onStartNode=false;
    int _currentIndex = 0;
    public override void OnEnter()
    {
        _startNode = fsm.enemy._startNode;
        _path = GameManager.instance.getPath(_startNode,fsm.enemy._toPatrol);
        //fsm.hunter.resting = true;
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del idle");
    }

    //el cazador para de moverse y recarga energias hasta que esté full
    public override void OnUpdate()
    {
        if()

    }

    bool goToNode(Node nodeToGo)
    {
        Debug.Log("Se ejecuta go to node nodetogo "+nodeToGo);
        if (nodeToGo != null)
        {
            Vector3 dir = nodeToGo.transform.position - fsm.enemy.transform.position;
            //dir.z = 0;
            fsm.enemy.transform.position += dir.normalized * Time.deltaTime * fsm.enemy._speed;

            if (dir.magnitude < 0.2f) return true;
            else return false;

        }else return false;
    }

}
