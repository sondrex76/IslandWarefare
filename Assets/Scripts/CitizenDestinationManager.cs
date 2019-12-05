using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

public class CitizenDestinationManager : MonoBehaviour
{
    [Range(0, 100)]
    public float _speed;
    bool _AgentHasReached = true;
    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    Queue<GraphNode> _path;
    GraphNode _home;
    GraphNode _work;
    GraphNode _shop;

    // Start is called before the first frame update
    void Start()
    {
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        _destination = new Vector3();
        _destination = transform.position;
        _path = new Queue<GraphNode>();
        _currentNode = _home;
        List<GraphNode> work = new List<GraphNode>();
        if (_graph.Nodes.Any())
        {
            foreach (var x in _graph.Nodes.Where(i => i._attribute == GraphNode.Attribute.Office))
            {
                work.Add(x);
            }
            System.Random randomWork = new System.Random();
            _work = work[randomWork.Next(0, work.Count)];
            _work.GetComponent<OfficeManager>().AddWorker(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _speed);
        if (_path != null)
        {

            
            if (_path.Count == 0 && _graph.Nodes.Any())
            {
                if (_currentNode == _home)
                {
                    _path = FindShortestPath(_home, _work);
                    _currentNode = _work;
                }
                else if (_currentNode == _work)
                {
                    List<GraphNode> shops = new List<GraphNode>();
                    foreach (var x in _graph.Nodes.Where(i => i._attribute == GraphNode.Attribute.Commnerical))
                    {
                        shops.Add(x);
                    }
                    System.Random randomShop = new System.Random();
                    _shop = shops[randomShop.Next(0, shops.Count)];
                    _path = FindShortestPath(_work, _shop);
                    _currentNode = _shop;
                }
                else if (_currentNode == _shop)
                {
                    _path = FindShortestPath(_shop, _home);
                    _currentNode = _home;
                }
            }
        }
        if (_AgentHasReached)
        {
            AStarWalk();
        }
        
        if (Vector3.SqrMagnitude(transform.position - _destination) <= 1.0f)
        {
            _AgentHasReached = true;
        }
        
    }

    public void SetHome(GraphNode home)
    {
        _home = home;
    }

    public void SetWork(GraphNode work)
    {
        _work = work;
    }


    void AStarWalk() {
      if (_path != null) {
        if (_path.Count > 0)
            {
                _destination = _path.Dequeue().transform.position;
                _AgentHasReached = false;
            }
      }
    }
    // Replace with GOAL based AI functionality
    // WIll just simulate how an agent would walk between nodes 
    GraphNode traverseNodesRandomly() 
    {
        var r = new System.Random();
        int i = r.Next(0, _graph.Nodes.Count);
        
        return _graph.Nodes[i].Adjacent[r.Next(0, _graph.Nodes[i].Adjacent.Count)];
        
    }

    int EuclideanEstimate(Vector3 node, Vector3 goal)
    {
        return (int) Mathf.Sqrt(Mathf.Pow(node.x - goal.x, 2) +
            Mathf.Pow(node.y - goal.y, 2) +
            Mathf.Pow(node.z - goal.z, 2));
    }
    IDictionary<GraphNode, GraphNode> nodeParents = new Dictionary<GraphNode, GraphNode>();

    Queue<GraphNode> FindShortestPath(GraphNode startNode, GraphNode goalNode) {
        Queue<GraphNode> path = new Queue<GraphNode>();
        GraphNode goal;

        goal = FindShortestPAthAStar(startNode, goalNode);
        if (goal == startNode || !nodeParents.ContainsKey(nodeParents[goal])) {
            Debug.Log("this is wrong");
            return null;
        }
        GraphNode curr = goal;
        while (curr != startNode) {
            path.Enqueue(curr);
            curr = nodeParents[curr];
        }
        path = new Queue<GraphNode>(path.Reverse());
        return path;
    }

    GraphNode FindShortestPAthAStar(GraphNode start, GraphNode goal)
    {
        nodeParents = new Dictionary<GraphNode, GraphNode>();
        uint nodeVisitCount = 0;
        float timeNow = Time.realtimeSinceStartup;

        IDictionary<GraphNode, int> heuristicScore = new Dictionary<GraphNode, int>();

        IDictionary<GraphNode, int> distanceFromStart = new Dictionary<GraphNode, int>();

        foreach(GraphNode node in _graph.Nodes)
        {
            heuristicScore.Add(new KeyValuePair<GraphNode, int>(node, int.MaxValue));
            distanceFromStart.Add(new KeyValuePair<GraphNode, int>(node, int.MaxValue));
        }

        heuristicScore[start] = EuclideanEstimate(start.transform.position, goal.transform.position);
        distanceFromStart[start] = 0;

        PriorityQueue<int, GraphNode> priorityQueue = new PriorityQueue<int, GraphNode>();
        priorityQueue.Enqueue(heuristicScore[start], start);
        while(priorityQueue.Count > 0)
        {
            // Get the node with the least distance from the start
            GraphNode curr = priorityQueue.Dequeue();
            nodeVisitCount++;

            // If our current node is the goal then stop
            if (curr == goal)
            {
                return goal;
            }


            foreach (GraphNode node in curr.Adjacent)
            {
                // Get the distance so far, add it to the distance to the neighbor
                int currScore = distanceFromStart[curr];

                // If our distance to this neighbor is LESS than another calculated shortest path
                //    to this neighbor, set a new node parent and update the scores as our current
                //    best for the path so far.
                if (currScore < distanceFromStart[node])
                {
                    nodeParents[node] = curr;
                    distanceFromStart[node] = currScore;

                    int hScore = distanceFromStart[node] + EuclideanEstimate(node.transform.position, goal.transform.position);
                    heuristicScore[node] = hScore;

                    // If this node isn't already in the queue, make sure to add it. Since the
                    //    algorithm is always looking for the smallest distance, any existing entry
                    //    would have a higher priority anyway.
                    if (!priorityQueue.Contains(node))
                    {
                      priorityQueue.Enqueue(hScore, node);
                    }
                }
            }
        }

        return start;
    }

}

class PriorityQueue<P, V>
{
    private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();

  public int Count {
    get { return list.Count; }
  }


     public void Enqueue(P priority, V value)
    {
        Queue<V> q;
        if (!list.TryGetValue(priority, out q))
        {
            q = new Queue<V>();
            list.Add(priority, q);
        }
        q.Enqueue(value);
    }
    public V Dequeue()
    {
        // will throw if there isn’t any first element!
        var pair = list.First();
        var v = pair.Value.Dequeue();
        if (pair.Value.Count == 0) // nothing left of the top priority.
            list.Remove(pair.Key);
        return v;
    }
    public bool IsEmpty
    {
        get { return !list.Any(); }
    }

  public bool Contains(V item) {
    foreach (var x in list.Where(i => EqualityComparer<V>.Equals(i.Key, item))) {
        return true;
    }
    return false;
  }
}
