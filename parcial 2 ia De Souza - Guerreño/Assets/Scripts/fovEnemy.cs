using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fovEnemy : MonoBehaviour
{

    [SerializeField] List<GameObject> _Enemigos = new List<GameObject>();
    [SerializeField] LayerMask WallLayer;
    [SerializeField] float viewRadius, viewAngle;

    void Update()
    {
        //LineOfSight();
        FieldOfView();
    }


    void FieldOfView()
    {
        foreach (GameObject enemy in _Enemigos)
        {
            Vector3 dir = enemy.transform.position - transform.position;
            if (dir.magnitude <= viewRadius)
            {
                if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
                {
                    if (!Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, WallLayer))
                    {
                        enemy.GetComponent<Renderer>().material.color = Color.red;
                        Debug.DrawLine(transform.position, enemy.transform.position);
                    }
                    else
                    {
                        enemy.GetComponent<Renderer>().material.color = Color.blue;
                        Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                    }


                        continue;
                }
            }
            enemy.GetComponent<Renderer>().material.color = Color.blue;
        }
    }
    void LineOfSight()
    {
        foreach (var enemy in _Enemigos)
        { 
            Vector3 dir = enemy.transform.position - transform.position;
            if (!Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, WallLayer))
            {
                enemy.GetComponent<Renderer>().material.color = Color.red;
                Debug.DrawLine(transform.position, enemy.transform.position);
            }
            else
            {
                enemy.GetComponent<Renderer>().material.color = Color.blue;
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 LineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 LineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);
        
        Gizmos.DrawLine(transform.position, transform.position + LineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + LineB * viewRadius);

    }
    
    Vector3 GetVectorFromAngle( float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
