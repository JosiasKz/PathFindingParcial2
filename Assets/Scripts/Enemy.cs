using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]List<Node> _patrolNodes = new List<Node>();
    [SerializeField] List<Node> _pathfindingNodes = new List<Node>();
    [SerializeField] float _speed;
    [SerializeField]LayerMask wallLayer;
    [SerializeField] float _searchRadius;
    bool pathFinding = false;
    Node _currentNode;

    //public void SetPos(Vector3 pos)
    //{
    //    //pos.z =transform.position.z;
    //    transform.position = pos;
    //}
    //public void SetPath(List<Node> p)
    //{
    //    _path = p;
    //    _path.Reverse();
    //}

    // Update is called once per frame
    void Update()
    {
        
        if (_patrolNodes.Count > 0)
        {
            if (!LineOfSight(_patrolNodes[0].transform))
            {
                Debug.Log("buscando otro nodo cercano");
                _currentNode = getClosestNode(_searchRadius);
            }
            else
            {
                Debug.Log("Nos quedamos con el primer nodo");

                _currentNode = _patrolNodes[0];
            }
        }

        if(_currentNode != null)
        {
            Vector3 dir = _currentNode.transform.position - transform.position;
            //dir.z = 0;
            transform.position += dir.normalized * Time.deltaTime * _speed;

            if (dir.magnitude < 0.2f)
            {
                _currentNode=null;
            }
        }
        //if (_path.Count > 0)
        //{



        //}    
    }

    bool LineOfSight(Transform target)
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

    Node getClosestNode(float searchRadius)
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
}
