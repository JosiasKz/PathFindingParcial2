using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitState : State
{
    Vector3 lastSeenPosition;
    public override void OnEnter()
    {
        //Debug.Log("Entro al Hunt");
        lastSeenPosition = GameManager.instance._player.transform.position;
    }

    public override void OnExit()
    {
        fsm.enemy._onPersuit=false;
        //Debug.Log("Salgo del Hunt");
    }

    public override void OnUpdate()
    {
        Transform player = GameManager.instance._player.transform;

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
    void goToPlayer(Transform target)
    {
        Vector3 dir = target.position - fsm.enemy.transform.position;
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

    void goToPosition(Vector3 pos)
    {
        Vector3 dir = pos - fsm.enemy.transform.position;
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
    public override void OnPlayerDetected(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON PLAYER DETECTED PERSUIT");
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);
        fsm.ChangeState(PlayerState.Persuit);
    }
    public override void OnAlertReceived(Vector3 pos)
    {
        Debug.Log(fsm.enemy.name + " ON ALERT RECEIVED PERSUIT");
        fsm.ChangeState(PlayerState.Reset);
        fsm.enemy._toPatrol = fsm.enemy.getClosestNodeFromPosition(pos);

    }
}
