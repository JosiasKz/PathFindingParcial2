using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    List<Node> _path =  new List<Node>();
    [SerializeField] float _speed;

    public void SetPos(Vector3 pos)
    {
        //pos.z =transform.position.z;
        transform.position = pos;
    }
    public void SetPath(List<Node>p)
    {
        _path = p;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (_path.Count > 0)
            {
                Vector3 dir = _path[0].transform.position - transform.position;
                //dir.z = 0;
                transform.position += dir.normalized * Time.deltaTime * _speed;

                if (dir.magnitude < 0.1f)
                {
                    _path.RemoveAt(0);
                }

            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }
}
