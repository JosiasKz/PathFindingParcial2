using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Node> GenerateDijkstra(Node startingNode, Node goalNode)
    {
        Debug.Log(startingNode, goalNode);
        //si no hay nodos seleccionados, rompe y vuelve
        if (startingNode == null || goalNode == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode, 0);

        //Se crea un diccionario de nodos, cameFrom
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                path.Reverse();
                return path;
            }

            foreach (Node next in current.GetNeightbours())
            {
                if (next.isBlocked) continue;

                float newCost = costSoFar[current] + next.cost;

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (costSoFar[next] > newCost)
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }

            }

        }
        return null;
    }

    public List<Node> GenerateBFS(Node startingNode, Node goalNode)
    {
        //si no hay nodos seleccionados, rompe y vuelve
        if (startingNode == null || goalNode == null) return null;

        //Se crea una queue de nodos y el primero en agregar es el startingNode
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        //Se crea un diccionario de nodos, cameFrom
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        //mientras la queue tenga valores
        while (frontier.Count > 0)
        {
            //tomamos al primer nodo de la queue y lo nombramos como el nodo actual
            Node current = frontier.Dequeue();

            //El nodo actual es el nodo final?
            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node node in path)
                {
                    node.PaintNode(Color.yellow);
                }
                path.Reverse();
                return path;
            }

            //Por cada nodo vecino del nodo actual
            foreach (Node next in current.GetNeightbours())
            {
                //Esta bloqueado? lo pasamos
                if (next.isBlocked) continue;
                //Si no existe en el diccionario
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);

                }
            }
        }
        return null;
    }

    //Esta funcion es la que traza el camino entre los nodos seleccionados
    public IEnumerator PaintBFS(Node startingNode, Node goalNode)
    {
        


        yield return null;
    }

    public IEnumerator PaintDijkstra(Node startingNode, Node goalNode)
    {
        Debug.Log(startingNode, goalNode);
        //si no hay nodos seleccionados, rompe y vuelve
        if (startingNode == null || goalNode == null) yield break;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode, 0);

        //Se crea un diccionario de nodos, cameFrom
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node,float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            current.PaintNode(Color.green);
            yield return new WaitForSeconds(0.1f);
            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node node in path)
                {
                    node.PaintNode(Color.yellow);
                }
                yield break;
            }

            foreach(Node next in current.GetNeightbours())
            {
                if (next.isBlocked) continue;

                float newCost = costSoFar[current]+next.cost;

                if (!costSoFar.ContainsKey(next))
                {
                    next.PaintNode(Color.blue);
                    frontier.Enqueue(next, newCost);
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (costSoFar[next]>newCost)
                {
                    frontier.Enqueue(next, newCost);
                    costSoFar[next]= newCost;
                    cameFrom[next]= current;
                }

            }

        }
        yield return null;
    }

    public IEnumerator PaintGreedyBFS(Node startingNode, Node goalNode)
    {
        if(startingNode == null || goalNode == null) yield break;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode,0);

        Dictionary<Node,Node> cameFrom = new Dictionary<Node,Node>();
        cameFrom.Add(startingNode,null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            current.PaintNode(Color.green);
            yield return new WaitForSeconds(0.1f);

            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node node in path)
                {
                    node.PaintNode(Color.yellow);
                }
                yield break;
            }

            foreach (Node next in current.GetNeightbours())
            {
                if (next.isBlocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next, Heuristic(next.transform.position,goalNode.transform.position));
                    
                    cameFrom.Add (next,current);

                    next.PaintNode(Color.blue);

                    yield return new WaitForSeconds(0.1f);
                }
            }

        }
        
    }

    public IEnumerator PaintAStar(Node startingNode, Node goalNode)
    {
        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode, 0);

        //Se crea un diccionario de nodos, cameFrom
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            current.PaintNode(Color.green);
            yield return new WaitForSeconds(0.1f);
            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node node in path)
                {
                    node.PaintNode(Color.yellow);
                }
                yield break;
            }

            foreach (Node next in current.GetNeightbours())
            {
                if(next.isBlocked) continue;

                float newCost = costSoFar[current] + next.cost;
                float dist = Heuristic(next.transform.position, goalNode.transform.position);
                float priority = newCost + dist;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next,priority);
                    cameFrom.Add(next,current);
                    costSoFar.Add(next, newCost);
                    next.PaintNode(Color.blue);

                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    if (costSoFar[next] > newCost)
                    {
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next]= newCost;
                    }
                }
            }

        }

     }

    public List<Node> GenerateAStar(Node startingNode, Node goalNode)
    {
        
        if (startingNode == null || goalNode == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode, 0);

        //Se crea un diccionario de nodos, cameFrom
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();
            //current.PaintNode(Color.green);
            //yield return new WaitForSeconds(0.1f);
            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }

                foreach (Node node in path)
                {
                    node.PaintNode(Color.yellow);
                }
                path.Reverse();
                return path;
            }

            foreach (Node next in current.GetNeightbours())
            {
                Debug.Log("NODO "+next+" NEIGHBOURS COUNT "+ current.GetNeightbours().Count);
                if (next.isBlocked)
                {  
                    continue;
                }

                float newCost = costSoFar[current] + next.cost;
                float dist = Heuristic(next.transform.position, goalNode.transform.position);
                float priority = newCost + dist;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                    //next.PaintNode(Color.blue);

                    //yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    if (costSoFar[next] > newCost)
                    {
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }
                }
            }

        }
        return null;
    }
    public List<Node> GenerateGreedyBFS(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                Debug.Log("listo");

                //Creamos una lista de nodos llamado path
                List<Node> path = new List<Node>();

                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }
                path.Reverse();
                return path;
            }

            foreach (Node next in current.GetNeightbours())
            {
                if (next.isBlocked) continue;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next, Heuristic(next.transform.position, goalNode.transform.position));

                    cameFrom.Add(next, current);

                    
                }
            }

        }
        return null;
    }
    public float Heuristic(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
    }

    public float ManhattanHeuristic(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(start.x - end.x)+Mathf.Abs(start.y - end.y);
    }
}
