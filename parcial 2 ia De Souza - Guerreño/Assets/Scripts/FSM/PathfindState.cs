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
        Debug.Log(fsm.enemy.name+" ON ENTER PATHFIND DESDE "+_startNode+" CON DESTINO A "+ fsm.enemy._toPatrol);

        ////Reiniciamos el patrullaje desde 0
        //if (!fsm.enemy._patrolNodes.Contains(fsm.enemy._toPatrol)|| fsm.enemy._toPatrol == null)
        //{
        //    fsm.enemy._toPatrol = fsm.enemy._patrolNodes[0];
        //}

        _path = GameManager.instance.getPath(_startNode,fsm.enemy._toPatrol);        
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del idle");
    }

    public override void OnUpdate()
    {
        if (!fsm.enemy.checkOnNodePosition(fsm.enemy._toPatrol.transform.position))
        {
            //Debug.Log("PATH COUNT "+_path.Count+" CUURRENT INDEX "+_currentIndex);
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
