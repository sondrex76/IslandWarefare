using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

public class CitizenDestinationManager : MonoBehaviour
{
    
    NavMeshAgent _agent;
    bool _AgentHasReached = true;
    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    GraphNode AStarStartTest;
    GraphNode AStarGoalTest;
    IList<GraphNode> _path;
    // Start is called before the first frame update
    void Start()
    {
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        _agent = GetComponent<NavMeshAgent>();
        _destination = new Vector3();
        _path = new List<GraphNode>();
        _currentNode = traverseNodesRandomly();
        AStarStartTest = _graph.Nodes[0];
        AStarGoalTest = _graph.Nodes[20];

      _path = FindShortestPath(AStarStartTest, AStarGoalTest);
      Debug.Log(_path.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (_AgentHasReached)
        {
            //RandomWalk();
            
            AStarWalk();
            
            
        }
        
        if (Vector3.SqrMagnitude(transform.position - _destination) <= 1.0f)
        {
            _AgentHasReached = true;
        }
        
    }
    void RandomWalk() {
        var r = new System.Random();
            
        _currentNode = _currentNode.Adjacent[r.Next(0, _currentNode.Adjacent.Count)];
        _destination = _currentNode.transform.position;
        _agent.destination = _destination;
        _AgentHasReached = false;
    }

    void AStarWalk() {
      if (_path != null) {
        if (_path.Count > 0)
            {
                _destination = _path[0].transform.position;
                _path.RemoveAt(0);
                _agent.destination = _destination;
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

    IList<GraphNode> FindShortestPath(GraphNode startNode, GraphNode goalNode) {
      IList<GraphNode> path = new List<GraphNode>();
      GraphNode goal;

      goal = FindShortestPAthAStar(startNode, goalNode);
      if (goal == startNode || !nodeParents.ContainsKey(nodeParents[goal])) {
        Debug.Log("this is wrong");
        return null;
      }
      GraphNode curr = goal;
      while (curr != startNode) {
        path.Add(curr);
        curr = nodeParents[curr];
      }

      return path;
    }

    GraphNode FindShortestPAthAStar(GraphNode start, GraphNode goal)
    {
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
