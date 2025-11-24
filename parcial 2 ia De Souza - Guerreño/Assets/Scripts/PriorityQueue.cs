using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue : MonoBehaviour
{
    Dictionary<Node,float> _allNodes = new Dictionary<Node,float>();

    public int Count
    {
        get { return _allNodes.Count; }
    }

    public void Enqueue(Node node, float cost)
    {
        if (_allNodes.ContainsKey(node))
        {
            _allNodes[node] = cost;
        }
        else
        {
            _allNodes.Add(node, cost);
        }
    }

    public Node Dequeue()
    {
        Node node = null;
        float cost = Mathf.Infinity;

        foreach (var item in _allNodes)
        {
            if(item.Value < cost)
            {
                cost=item.Value;
                node =item.Key;
            }
        }
        _allNodes.Remove(node);
        return node;
    }

}
