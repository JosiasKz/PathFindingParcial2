using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PathfindState : State
{
    List<Node> _path = new List<Node>();
    Node _startNode;
    bool _onStartNode=false;
    public override void OnEnter()
    {
        _startNode = fsm.enemy.getClosestNode(fsm.enemy._speed);
        //fsm.hunter.resting = true;
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del idle");
    }

    //el cazador para de moverse y recarga energias hasta que esté full
    public override void OnUpdate()
    {
        if (!_onStartNode)
        {
            _onStartNode = goToNode(_startNode);
        }
        else
        {
            Debug.Log("start node alcanzado");
        }
    }

    bool goToNode(Node nodeToGo)
    {
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
