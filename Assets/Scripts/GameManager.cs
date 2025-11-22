using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //tenemos dos nodos importantes, el start node y el goalnode(inicio y punto de llegada)
    Node startNode, goalNode;
    public static GameManager instance;
    Pathfinding _pf;
    [SerializeField] Player _player;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        //Arranca y crea un objeto pathfinding
        _pf = new Pathfinding();
    }

    public void Update()
    {//Si presionamos la tecla de espacio empezamos una corrutina de la funcion paintBFS del objeto pathfinding
        //es necesario ya tener los dos nodos elegidos
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(_pf.PaintBFS(startNode, goalNode));
            //_player.SetPath(_pf.GenerateBFS(startNode,goalNode));
            //StartCoroutine(_pf.PaintDijkstra(startNode,goalNode));}
            //_player.SetPath(_pf.GenerateDijkstra(startNode, goalNode));
            //StartCoroutine(_pf.PaintGreedyBFS(startNode, goalNode));
            //_player.SetPath(_pf.GenerateGreedyBFS(startNode, goalNode));
            StartCoroutine(_pf.PaintAStar(startNode, goalNode));
            _player.SetPath(_pf.GenerateAStar(startNode, goalNode));

        }
    }

    //Cuando llamamos a esta funcion, estamos declarando a un nodo como el nodo inicial
    public void SetStartNode(Node node)
    {

        if (startNode != null) startNode.GetComponent<Renderer>().material.color = Color.white;
        _player.SetPos(node.transform.position);

        startNode = node;
        node.GetComponent<Renderer>().material.color = Color.green;
    }
    //Y cuando llamamos a este estamos declarando al nodo como el nodo final(o de llegada, meta, etc)
    public void SetGoalNode(Node node)
    {
        if (goalNode != null) goalNode.GetComponent<Renderer>().material.color = Color.white;
        goalNode = node;
        node.GetComponent<Renderer>().material.color = Color.red;
    }

}
