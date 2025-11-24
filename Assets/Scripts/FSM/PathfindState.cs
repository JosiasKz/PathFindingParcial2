using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        _currentIndex = 0;
        _startNode = fsm.enemy._startNode;
        Debug.Log("ON ENTER PATHFIND "+_startNode+" TO PATROL "+ fsm.enemy._toPatrol);
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
        if (!fsm.enemy.checkOnNodePosition(fsm.enemy._toPatrol.transform.position))
        {
            Debug.Log("PATH COUNT "+_path.Count+" CUURRENT INDEX "+_currentIndex);
            if (fsm.enemy.LineOfSight(_path[_currentIndex].transform))
            {
                goToNode(_path[_currentIndex]);
            }
            else
            {
                fsm.ChangeState(PlayerState.Reset);
            }
        }
        else
        {
            fsm.ChangeState(PlayerState.Patrol);
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
                _currentIndex++;
            }
        }
    }

}
