using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este estado se encarga de perseguir al player, si lo pierde de vista va hacia el ultimo punto donde lo vió por ultima vez
public class PersuitState : State
{
    Vector3 lastSeenPosition;
    public override void OnEnter()
    {
        lastSeenPosition = GameManager.instance._player.transform.position;
    }

    public override void OnExit()
    {
        fsm.enemy._onPersuit=false;
    }

    public override void OnUpdate()
    {
        Transform player = GameManager.instance._player.transform;
        //Se usa line of sight para tener todo el tiempo referencia de donde está y perseguirlo
        if (fsm.enemy.LineOfSight(player))
        {
            lastSeenPosition = player.position;
            goToPlayer(player);
        }
        else
        {
            // perdió al player, buscar hasta el último punto visto
            if (!fsm.enemy.checkOnNodePosition(lastSeenPosition))
            {
                goToPosition(lastSeenPosition);
            }
            else
            {
                // Llegó al punto pero no ve al player, reset                
                fsm.ChangeState(PlayerState.Reset);                
            }
        }
    }
    //Sirve para perseguir al jugador pero teniendo un minimo de distancia, para no tirarse encima
    void goToPlayer(Transform target)
    {
        Vector3 dir = target.position - fsm.enemy.transform.position;
        float dist = dir.magnitude;

        if (dist <= fsm.enemy.minDistanceToPlayer)
            return;

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            fsm.enemy.transform.rotation = Quaternion.Lerp(
                fsm.enemy.transform.rotation,
                targetRot,
                Time.deltaTime * 8f
            );
        }
        dir /= dist; 

        float step = fsm.enemy._speed * Time.deltaTime;

        float maxAllowedStep = dist - fsm.enemy.minDistanceToPlayer;
        if (step > maxAllowedStep)
            step = maxAllowedStep;
        fsm.enemy.transform.position += dir * step;
    }
    //Sirve para ir a la ultima posición conocida del player
    void goToPosition(Vector3 pos)
    {
        Vector3 dir = pos - fsm.enemy.transform.position;
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            fsm.enemy.transform.rotation = Quaternion.Lerp(
                fsm.enemy.transform.rotation,
                targetRot,
                Time.deltaTime * 8f
            );
        }
        fsm.enemy.transform.position += dir.normalized * Time.deltaTime * fsm.enemy._speed;
    }

    //En caso de que se ejecute el evento de deteccion del player, interrumpe este estado para cambiar al de Persuit
    public override void OnPlayerDetected(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON PLAYER DETECTED PERSUIT");
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
