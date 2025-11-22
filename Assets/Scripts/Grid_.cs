using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_ : MonoBehaviour
{
    //La grilla se configura con un alto y un ancho
    [SerializeField] int _width;
    [SerializeField] int _height;
    //Offset para hacer una separación entre los nodos
    [SerializeField] float _offset;
    //Prefab del nodo
    [SerializeField] GameObject _cube;

    //un array bidimensional de nodos(la grilla en si)
    Node[,] _grid;


    // Start is called before the first frame update
    void Start()
    {
        //Se crea la grilla
        _grid = new Node[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Se instancian los cubos por cada espacio en la grilla
                GameObject cube = Instantiate(_cube, transform);
                cube.transform.position = new Vector3(x + (x * _offset), y + (y * _offset), 0);
                cube.GetComponent<Node>().Initialize(x, y, this);
                //En la grilla se guarda una referencia a cada nodo
                _grid[x, y] = cube.GetComponent<Node>();
            }
        }


    }
    //Con esto le pedimos a la grilla que nos devuelva un nodo especifico pasandole sus coordenadas
    public Node GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return null;

        return _grid[x, y];
    }
}
