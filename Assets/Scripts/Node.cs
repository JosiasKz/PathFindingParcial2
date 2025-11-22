using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    //El nodo tiene una lista de nodos vecinos
    [SerializeField] List<Node> _neighbours = new List<Node>();
    //Su posicion en x y en y
    public int _x;
    public int _y;
    //referencia a la misma grilla que lo va a contener
    public Grid_ _grid;
    //Esto marca si el nodo va ser nodo normal u obstaculo
    public bool isBlocked = false;
    public int cost;
    [SerializeField] TextMeshProUGUI _text;
    //La grilla llama a este metodo para inicializar al nodo, dandole su posición, la referencia a si misma y el nombre
    public void Initialize(int x, int y, Grid_ grid)
    {
        _grid = grid;
        _x = x;
        _y = y;
        gameObject.name = "Node: " + x + ", " + y;
    }

    public void Start()
    {
        SetCost(1);
    }
    //Este metodo le da el valor a la lista de vecinos
    public List<Node> GetNeightbours()
    {
        //Si la lista ya está populada, la devolvemos tal cual está sin hacer ninguna busqueda
        if (_neighbours.Count > 0) return _neighbours;


        //La grilla(grid) tiene una función llamada GetNode la cual devuelve un nodo dandole la posición indicada
        //Esta, GetNeighbours, lo que hace es decirle a la grilla que le devuelva 4 nodos teniendo en cuenta la posicion actual
        //Es decir, este nodo eta en la posición central, por lo tanto le va a pedir sus vecinos de arriba, abajo, izquieda y derecha
        //Cada nodo tendria su lista con sus potenciales 4 vecinos
        Node neighbour = _grid.GetNode(_x, _y + 1);//Arriba
        if (neighbour != null) _neighbours.Add(neighbour);
        neighbour = _grid.GetNode(_x - 1, _y);//Izquierda
        if (neighbour != null) _neighbours.Add(neighbour);
        neighbour = _grid.GetNode(_x, _y - 1);//Abajo
        if (neighbour != null) _neighbours.Add(neighbour);
        neighbour = _grid.GetNode(_x + 1, _y);//
        if (neighbour != null) _neighbours.Add(neighbour);

        return _neighbours;
    }
    //simplemente una funcion que pinta el renderer del nodo actual
    public void PaintNode(Color newColor)
    {
        gameObject.GetComponent<Renderer>().material.color = newColor;
    }
    //Esto permite que se pueda interactuar con el nodo usando el mouse
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SetStartNode(this);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.SetGoalNode(this);
        }
        if (Input.GetMouseButtonDown(2))
        {
            isBlocked = !isBlocked;

            PaintNode(isBlocked ? Color.grey : Color.white);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SetCost(cost + 1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SetCost(cost - 1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetCost(1);
        }
    }

    void SetCost(int newCost)
    {
        cost = Mathf.Clamp(newCost, 1, 99);
        _text.text = cost.ToString();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        foreach (Node n in _neighbours)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}