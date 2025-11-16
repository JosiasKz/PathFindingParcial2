using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Node startNode, goalNode;
    public static GameManager instance;
    
    
    
    private void Start()
    {
        if (instance == null) instance = this;
        Destroy(this); 
    }
    public void SetStartNode(Node node) 
    {
        if(startNode != null) startNode.GetComponent<Renderer>().material.color = Color.white;
        startNode = node;
        node.GetComponent<Renderer>().material.color = Color.green;
    }
    public void SetGoalNode(Node node) 
    {
        if (goalNode != null) goalNode.GetComponent<Renderer>().material.color = Color.white;
        goalNode = node;
        node.GetComponent<Renderer>().material.color = Color.red;
    }



}
