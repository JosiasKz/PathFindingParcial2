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
    //Esta funcion sirve para pedirle a A* que nos arme un camino optimo entre un nodo inicial y un nodo final
    public List<Node> getPath(Node startNode, Node goalNode)
    {
        return _pf.GenerateAStar(startNode, goalNode);
    }
    //Con este metodo iteramos por nuestra lista de enemigos y les ejecutamos su metodo ReceiveAlert, que incluye una posición donde el enemy tiene que ir
    public void AlertAllEnemies(Vector3 alertPosition)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.ReceiveAlert(alertPosition);
        }
    }
}
