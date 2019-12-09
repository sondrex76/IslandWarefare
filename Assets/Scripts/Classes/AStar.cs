using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    static int ManhattanEstimate(Vector3 node, Vector3 goal)
    {
        return (int)Mathf.Abs(Mathf.Abs(node.x - goal.x) +
            Mathf.Abs(node.y - goal.y) +
            Mathf.Abs(node.z - goal.z));
    }
    static Dictionary<GraphNode, GraphNode> nodeParents = new Dictionary<GraphNode, GraphNode>();

    public static Queue<GraphNode> FindShortestPath(GraphNode startNode, GraphNode goalNode, Graph graph)
    {
        Queue<GraphNode> path = new Queue<GraphNode>();
        GraphNode goal;

        goal = FindShortestPAthAStar(startNode, goalNode, graph);
        if (!nodeParents.ContainsKey(nodeParents[goal]))
        {
            //Debug.Log("this is wrong");
            return null;
        }
        GraphNode curr = goal;
        while (curr != startNode)
        {
            path.Enqueue(curr);
            curr = nodeParents[curr];
        }
        path = new Queue<GraphNode>(path.Reverse());
        return path;
    }

    static GraphNode FindShortestPAthAStar(GraphNode start, GraphNode goal, Graph graph)
    {
        nodeParents = new Dictionary<GraphNode, GraphNode>();

        foreach (GraphNode node in graph.Nodes)
        {
            node.heuristicScore = int.MaxValue;
            node.distanceFromStart = int.MaxValue;
        }

        start.heuristicScore = ManhattanEstimate(start.transform.position, goal.transform.position);
        start.distanceFromStart = 0;

        //PriorityQueue<int, GraphNode> priorityQueue = new PriorityQueue<int, GraphNode>();

        Queue<GraphNode> open = new Queue<GraphNode>();
        HashSet<GraphNode> closed = new HashSet<GraphNode>();

        open.Enqueue(start);
        
        while (open.Count > 0)
        {
            // Get the node with the least distance from the start
            GraphNode curr = open.Dequeue();
            closed.Add(start);

            // If our current node is the goal then stop
            if (curr == goal)
            {
                return goal;
            }


            foreach (GraphNode node in curr.Adjacent)
            {
                // Get the distance so far, add it to the distance to the neighbor
                int currScore = curr.distanceFromStart;

                // If our distance to this neighbor is LESS than another calculated shortest path
                //    to this neighbor, set a new node parent and update the scores as our current
                //    best for the path so far.
                if (currScore < node.distanceFromStart)
                {
                    nodeParents[node] = curr;
                    node.distanceFromStart = currScore;

                    int hScore = node.distanceFromStart + ManhattanEstimate(node.transform.position, goal.transform.position);
                    node.heuristicScore = hScore;

                    // If this node isn't already in the queue, make sure to add it. Since the
                    //    algorithm is always looking for the smallest distance, any existing entry
                    //    would have a higher priority anyway.
                    if (!closed.Contains(node))
                    {
                        open.Enqueue(node);
                    }
                }
            }
        }

        return start;
    }

}
