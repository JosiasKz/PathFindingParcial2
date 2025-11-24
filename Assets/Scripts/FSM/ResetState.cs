using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : State
{
    Node _startNode;
    public override void OnEnter()
    {
        Debug.Log("ON ENTER reset");
        _startNode = fsm.enemy.getClosestNode(fsm.enemy._searchRadius);
        //Debug.Log("Entro al Hunt");
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del Hunt");
    }

    public override void OnUpdate()
    {
        if (_startNode == null || !fsm.enemy.LineOfSight(_startNode.transform))
        {
            Debug.Log("NO HAY NODO ENCONTRADO PARA ARRANCAR");
            _startNode = fsm.enemy.getClosestNode(fsm.enemy._searchRadius*2);
        }
        else
        {
            if (!fsm.enemy.checkOnNodePosition(_startNode.transform.position))
            {
                goToNode(_startNode);
            }
            else
            {
                Debug.Log("start node alcanzado");
                fsm.enemy._startNode = _startNode;
                fsm.ChangeState(PlayerState.Pathfinding);
            }
        }

    }

    void goToNode(Node nodeToGo)
    {
        Debug.Log("Se ejecuta go to node nodetogo " + nodeToGo);
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
        }
    }

}
