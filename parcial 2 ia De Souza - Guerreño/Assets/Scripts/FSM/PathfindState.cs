using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//Este state se encarga de recorrer el _path entregado por el algoritmo A*
public class PathfindState : State
{
    List<Node> _path = new List<Node>();
    Node _startNode;
    bool _onStartNode=false;
    int _currentIndex = 0;
    public override void OnEnter()
    {
        _currentIndex = 0;
        //El startNode es el nodo que nos encontró el state Reset
        _startNode = fsm.enemy._startNode;
        Debug.Log(fsm.enemy.name+" ON ENTER PATHFIND DESDE "+_startNode+" CON DESTINO A "+ fsm.enemy._toPatrol);
        //Acá le pedimos a A* que nos entregue un camino, usando como startNode _StartNode y como goalNode lo que tengamos en _toPatrol
        _path = GameManager.instance.getPath(_startNode,fsm.enemy._toPatrol);        
    }

    public override void OnExit()
    {
        //Debug.Log("Salgo del idle");
    }

    public override void OnUpdate()
    {
        //Mientras no hayamos llegado al nodo destino
        if (!fsm.enemy.checkOnNodePosition(fsm.enemy._toPatrol.transform.position))
        {
            //Hacemos lineOfSight para ver si el nodo actual al que estamos yendo está a la vista
            if (fsm.enemy.LineOfSight(_path[_currentIndex].transform))
            {
                //Si está a la vista, el enemy agarra viaje
                goToNode(_path[_currentIndex]);
            }
            else
            {
                //Si no, vuelve a reset state para recalcular donde ir
                fsm.ChangeState(PlayerState.Reset);
            }
        }
        else
        {
            //Si el enemy llegó a destino, significa que hay que patrullar
            fsm.ChangeState(PlayerState.Patrol);
        }
    }

    //Con este metodo el enemy se mueve de nodo a nodo
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

    //En caso de que se ejecute el evento de deteccion del player, interrumpe este estado para cambiar al de Persuit
    public override void OnPlayerDetected(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON PLAYER DETECTED PATHFIND");
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
