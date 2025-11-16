using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] float _offset;
    [SerializeField] GameObject _cube;

    Node[,] _grid; 
    void Start()
    {
        _grid = new Node[_width, _height];
   
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject newCube = Instantiate(_cube, transform);
                newCube.transform.position = new Vector3(x + (x * _offset), y + (y * _offset), 0);  
                newCube.GetComponent<Node>().Initialize(x, y, this);
                _grid[x,y] = newCube.GetComponent<Node>();

            }
        }
        foreach (Node node in _grid)
        {
            node.GetNeighbours();
        }
    }

    public Node GetNode(int x, int y) 
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height)
            return null;
        
        return _grid[x,y];
    
    }



}

