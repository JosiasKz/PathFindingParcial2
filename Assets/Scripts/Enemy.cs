using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public List<Node> _patrolNodes = new List<Node>();
    [SerializeField] List<Node> _pathfindingNodes = new List<Node>();
    [SerializeField] public float _speed;
    [SerializeField]LayerMask wallLayer;
    [SerializeField] float _searchRadius;
    FiniteStateMachine fsm;
    bool pathFinding = false;
    Node _currentNode;
    [SerializeField] public TextMeshProUGUI stateText;
    private void Start()
    {
        //Inicializamos state machine
        fsm = new FiniteStateMachine(this);
        fsm.AddState(PlayerState.Pathfinding, new PathfindState());
        fsm.AddState(PlayerState.Patrol, new Patrolstate());
        fsm.AddState(PlayerState.Persuit, new PersuitState());
        //Arranca patrol
        fsm.ChangeState(PlayerState.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,_searchRadius);
    }
}
