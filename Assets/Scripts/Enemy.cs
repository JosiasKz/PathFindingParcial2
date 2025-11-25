using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    //Lista de nodos que tiene que patrullar
    public List<Node> _patrolNodes = new List<Node>();
    public float _speed;
    [SerializeField] LayerMask wallLayer;
    public float _searchRadius;
    [SerializeField] float viewRadius, viewAngle;
    FiniteStateMachine fsm;
    //_startNode es el node que va a usar el enemy para empezar el path que va a generar A*
    public Node _startNode;
    //_toPatrol es un nodo temporal, sirve para tener de referencia en que nodo se quedó antes de ser interrumpido
    public Node _toPatrol;
    public TextMeshProUGUI stateText;
    public bool _onPersuit =false;
    //OnDetected es el evento que lanza enemy cuando detecta al player
    public event Action<Enemy, Vector3> OnPlayerDetected;
    //OnAlertReceived es el evento que lanza enemy cuando recivió el alerta del game manager
    public event Action<Enemy, Vector3> OnAlertReceived;
    //Distancia minima que va a tomar el enemigo del player para no encimarlo
    public float minDistanceToPlayer;
    private void Start()
    {
        //Inicializamos state machine
        fsm = new FiniteStateMachine(this);
        fsm.AddState(PlayerState.Pathfinding, new PathfindState());
        fsm.AddState(PlayerState.Patrol, new Patrolstate());
        fsm.AddState(PlayerState.Persuit, new PersuitState());
        fsm.AddState(PlayerState.Reset, new ResetState());
        //Arranca patrol
        fsm.ChangeState(PlayerState.Patrol);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(name + " TO PATROL "+_toPatrol);

        fsm.Update();
        //El enemigo chequea FOV todo el tiempo para ver si se cruza con el player
        if (FieldOfView() && !_onPersuit)
        {
            Debug.Log(name+ " envia alerta");
            //En caso de encontrar al player, guarda la posición del mismo en la variable pos
            //Y ejecuta AlertAllEnemies en el gameManager
            Vector3 pos = GameManager.instance._player.transform.position;
            GameManager.instance.AlertAllEnemies(pos);
            //Tambien ejecutamos el evento OnPlayerDetected, al cual el fsm está suscrito
            //permitiendole saber cuando hacer la transición al estado Persuit
            OnPlayerDetected?.Invoke(this, pos);
            _onPersuit=true;
        }
    }

    public bool LineOfSight(Transform target)
    {
        Vector3 dir = target.transform.position - transform.position;
        if (!Physics.Raycast(transform.position,dir, out RaycastHit hitInfo,dir.magnitude, wallLayer ))
        {
            Debug.DrawLine(transform.position,target.transform.position);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position,hitInfo.point,Color.red);
            return false;
        }
    }
    bool FieldOfView()
    {
        Vector3 dir = GameManager.instance._player.transform.position - transform.position;
        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(transform.forward, dir) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, wallLayer))
                {
                    Debug.DrawLine(transform.position, GameManager.instance._player.transform.position,Color.yellow);
                    return true;
                }
                else
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                    return false;
                }

            }
        }
        return false;
    }

    //Esta función se encarga de devolver cual es el nodo más cercano al enemigo
    public Node getClosestNode(float searchRadius)
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, searchRadius);
        Node closest = null;
        
        float minDist = Mathf.Infinity;

        foreach (Collider obj in objects)
        {
            Node node = obj.GetComponent<Node>();
            if (node==null) continue;
            if (!LineOfSight(obj.transform)) continue;
            float dist = Vector3.Distance(transform.position, node.transform.position);

            if (dist < minDist)
            {
                minDist = dist;

                closest = node;
            }
        }
        return closest;
    }
    //Esta función se encarga de buscar el nodo más cercano pero a partir de una posición
    public Node getClosestNodeFromPosition(Vector3 alertPos)
    {
        Collider[] hits = Physics.OverlapSphere(alertPos, _searchRadius * 2);
        Node closest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider col in hits)
        {
            Node n = col.GetComponent<Node>();
            if (n == null) continue;
            //if (!LineOfSight(col.transform)) continue;
            float d = Vector3.Distance(alertPos, n.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = n;
            }
        }

        return closest;
    }

    //ESta función sirve para ver si algo llegó a un nodo en especifico
    public bool checkOnNodePosition(Vector3 nodePosition)
    {
        Vector3 dir = nodePosition - transform.position;

        if (dir.magnitude < 0.2f) return true;
        else return false;
    }

    //ESta función sirve para ver si algo llegó a un nodo en especifico
    public void ReceiveAlert(Vector3 alertPos)
    {
        // No cambiar si ya lo está persiguiendo
        if (fsm.currentPS == PlayerState.Persuit) return;
        OnAlertReceived?.Invoke(this,alertPos);
        Debug.Log(name + " recivió alerta para ir hacia el nodo " + _toPatrol);
    }

    //Con esto podemos visualizar los rangos de de busqueda
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 LineA = GetVectorFromAngle(viewAngle / 2 + transform.eulerAngles.y);
        Vector3 LineB = GetVectorFromAngle(-viewAngle / 2 + transform.eulerAngles.y);

        Debug.DrawLine(transform.position, transform.position + LineA * viewRadius,Color.blue);
        Debug.DrawLine(transform.position, transform.position + LineB * viewRadius, Color.blue);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
