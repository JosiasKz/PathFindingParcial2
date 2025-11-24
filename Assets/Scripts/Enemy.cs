using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List<Node> _patrolNodes = new List<Node>();
    [SerializeField] List<Node> _pathfindingNodes = new List<Node>();
    [SerializeField] public float _speed;
    [SerializeField]LayerMask wallLayer;
    [SerializeField] public float _searchRadius;
    [SerializeField] float viewRadius, viewAngle;
    FiniteStateMachine fsm;
    public Node _startNode;
    bool pathFinding = false;
    Node _currentNode;
    public Node _toPatrol;
    [SerializeField] public TextMeshProUGUI stateText;
    Player _player;
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
        //_player = GameManager.instance._player;

    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
        FieldOfView();
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
    void FieldOfView()
    {
        Vector3 dir = GameManager.instance._player.transform.position - transform.position;
        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, wallLayer))
                {
                    //_player.GetComponent<Renderer>().material.color = Color.red;
                    Debug.DrawLine(transform.position, GameManager.instance._player.transform.position,Color.green);
                }
                else
                {
                    //_player.GetComponent<Renderer>().material.color = Color.blue;
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                }

            }
        }
        //_player.GetComponent<Renderer>().material.color = Color.blue;
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

    public bool checkOnNodePosition(Vector3 nodePosition)
    {
        Vector3 dir = nodePosition - transform.position;

        if (dir.magnitude < 0.2f) return true;
        else return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,_searchRadius);
    }
}
