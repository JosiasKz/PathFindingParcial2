using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este state se encarga de redirigir al enemy de nuevo al grafo, tanto para retomar el patrullaje o para ir al punto de alerta
public class ResetState : State
{
    Node _startNode;
    public override void OnEnter()
    {
        //Al entrar, inmediatamente buscamos el nodo más cercano
        Debug.Log(fsm.enemy.name+" ON ENTER reset TOPATROL "+fsm.enemy._toPatrol);
        _startNode = fsm.enemy.getClosestNode(fsm.enemy._searchRadius);
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del Hunt");
    }

    public override void OnUpdate()
    {
        //si no tenemos el nodo o si no lo podemos ver, buscamos de nuevo pero hacemos el rango de busqueda más grande
        if (_startNode == null || !fsm.enemy.LineOfSight(_startNode.transform))
        {
            Debug.Log(fsm.enemy+ " no encontró nodo cercano para hacer reset, como fallback ampliamos el radio de busqueda");
            _startNode = fsm.enemy.getClosestNode(fsm.enemy._searchRadius*4);
        }
        else
        {
            //Mientras el enemy no esté en el nodo, se desplaza hacia el hasta llegar
            if (!fsm.enemy.checkOnNodePosition(_startNode.transform.position))
            {
                goToNode(_startNode);
            }
            else
            {
                //Si llegamos al nodo, hacemos pathfinding State
                Debug.Log(fsm.enemy + " start node alcanzado");
                fsm.enemy._startNode = _startNode;
                fsm.ChangeState(PlayerState.Pathfinding);
            }
        }

    }

    //Este metodo se usa para simplemente desplazarse hacia el nodo
    void goToNode(Node nodeToGo)
    {
        Debug.Log(fsm.enemy.name+" ejecuta go to node nodetogo " + nodeToGo+" con destino final de "+fsm.enemy._toPatrol);
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

    //En caso de que se ejecute el evento de deteccion del player, interrumpe este estado para cambiar al de Persuit
    public override void OnPlayerDetected(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON PLAYER DETECTED RESET");
        fsm.ChangeState(PlayerState.Persuit);
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);

    }
    //En caso de que se ejecute el evento de alerta recibida, interrumpe este estado para cambiar al de Reset con destino al nodo más cercano donde se vió al player

    public override void OnAlertReceived(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON ALERT RECEIVED PERSUIT");
        fsm.ChangeState(PlayerState.Reset);
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);

    }
}
