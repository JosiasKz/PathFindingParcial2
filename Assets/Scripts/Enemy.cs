using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List<Node> _patrolNodes = new List<Node>();
    [SerializeField] public float _speed;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] public float _searchRadius;
    [SerializeField] float viewRadius, viewAngle;
    FiniteStateMachine fsm;
    public Node _startNode;
    public Node _toPatrol;
    [SerializeField] public TextMeshProUGUI stateText;
    public bool _onPersuit =false;
    Player _player;
    public event Action<Enemy, Vector3> OnPlayerDetected;
    private void Start()
    {
        //Inicializamos state machine
        fsm = new FiniteStateMachine(this);
        fsm.AddState(PlayerState.Pathfinding, new PathfindState());
        fsm.AddState(PlayerState.Patrol, new Patrolstate());
        fsm.AddState(PlayerState.Persuit, new PersuitState());
        fsm.AddState(PlayerState.Reset, new ResetState());
        //Arranca patrol
        fsm.ChangeState(PlayerState.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(name + " TO PATROL "+_toPatrol);
        fsm.Update();
        if (FieldOfView() && !_onPersuit)
        {
            Debug.Log(name+ " envia alerta");
            Vector3 pos = GameManager.instance._player.transform.position;
            GameManager.instance.AlertAllEnemies(pos);
            OnPlayerDetected?.Invoke(this, pos);
            fsm.ChangeState(PlayerState.Persuit);
            _onPersuit=true;
        }
    }

    public bool LineOfSight(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        if (!Physics.Raycast(transform.position,dir, out RaycastHit hitInfo,dir.magnitude, wallLayer ))
        {
            Debug.DrawLine(transform.position,target.transform.position);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position,hitInfo.point,Color.red);
            return false;
        }
    }
    bool FieldOfView()
    {
        Vector3 dir = GameManager.instance._player.transform.position - transform.position;
        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, wallLayer))
                {
                    Debug.DrawLine(transform.position, GameManager.instance._player.transform.position,Color.yellow);
                    return true;
                }
                else
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                    return false;
                }

            }
        }
        return false;
    }
    public Node getClosestNode(float searchRadius)
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, searchRadius);
        Node closest = null;
        
        float minDist = Mathf.Infinity;

        foreach (Collider obj in objects)
        {
            Node node = obj.GetComponent<Node>();
            if (node==null) continue;
            if (!LineOfSight(obj.transform)) continue;
            float dist = Vector3.Distance(transform.position, node.transform.position);

            if (dist < minDist)
            {
                minDist = dist;

                closest = node;
            }
        }
        return closest;
    }

    Node getClosestNodeFromPosition(Vector3 alertPos)
    {
        Collider[] hits = Physics.OverlapSphere(alertPos, _searchRadius * 2);
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider col in hits)
        {
            Node n = col.GetComponent<Node>();
            if (n == null) continue;

            float d = Vector3.Distance(alertPos, n.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = n;
            }
        }

        return closest;
    }

    public bool checkOnNodePosition(Vector3 nodePosition)
    {
        Vector3 dir = nodePosition - transform.position;

        if (dir.magnitude < 0.2f) return true;
        else return false;
    }

    public void ReceiveAlert(Vector3 alertPos)
    {
        // No cambiar si ya lo está persiguiendo
        if (fsm.currentPS == PlayerState.Persuit) return;
        // Saltamos al reset para conectarnos al grafo
        fsm.ChangeState(PlayerState.Reset);
        // Guardamos el punto como “ToPatrol temporal”
        _toPatrol = getClosestNodeFromPosition(alertPos);
        Debug.Log(name + " recivió alerta para ir hacia el nodo " + _toPatrol);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 LineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 LineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Debug.DrawLine(transform.position, transform.position + LineA * viewRadius,Color.blue);
        Debug.DrawLine(transform.position, transform.position + LineB * viewRadius, Color.blue);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
