using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighbours = new List<Node>();
    int _x;
    int _y;
    Grid _grid;

    public void Initialize(int x, int y, Grid grid)
    {
        _grid = grid;
        _x = x;
        _y = y;
        gameObject.name = "Node" + x + ", " + y; 

    }
    public void GetNeighbours()
    { 
        
        
        Node neighbor = _grid.GetNode(_x + 1, _y); //Derecha
        if (neighbor != null ) _neighbours.Add(neighbor);    //Esto llama a los nodos familiares de los costados

        neighbor = _grid.GetNode(_x - 1, _y); //Izquierda
        if (neighbor != null) _neighbours.Add(neighbor);
        neighbor = _grid.GetNode(_x , _y + 1); //Arriba
        if (neighbor != null) _neighbours.Add(neighbor);
        neighbor = _grid.GetNode(_x , _y - 1); //Abajo
        if (neighbor != null) _neighbours.Add(neighbor);

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //Este codigo detecta el precionar el mouse (0 = click izq, 1= click der, 2 = click ruedita)
        {
            GameManager.instance.SetStartNode(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.SetGoalNode(this);
        }

    }



}

