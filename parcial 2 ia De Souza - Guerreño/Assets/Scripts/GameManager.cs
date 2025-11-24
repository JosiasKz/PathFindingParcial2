using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //tenemos dos nodos importantes, el start node y el goalnode(inicio y punto de llegada)
    public static GameManager instance;
    Pathfinding _pf;
    public Player _player;
    public List<Enemy> enemies = new List<Enemy>();
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
    //Cuando llamamos a esta funcion, estamos declarando a un nodo como el nodo inicial
    public List<Node> getPath(Node startNode, Node goalNode)
    {
        return _pf.GenerateAStar(startNode, goalNode);
    }
    public void AlertAllEnemies(Vector3 alertPosition)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.ReceiveAlert(alertPosition);
        }
    }
}
