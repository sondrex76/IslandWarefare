using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    static int EuclideanEstimate(Vector3 node, Vector3 goal)
    {
        return (int)Mathf.Sqrt(Mathf.Pow(node.x - goal.x, 2) +
            Mathf.Pow(node.y - goal.y, 2) +
            Mathf.Pow(node.z - goal.z, 2));
    }
    static IDictionary<GraphNode, GraphNode> nodeParents = new Dictionary<GraphNode, GraphNode>();

    public static Queue<GraphNode> FindShortestPath(GraphNode startNode, GraphNode goalNode, Graph graph)
    {
        Queue<GraphNode> path = new Queue<GraphNode>();
        GraphNode goal;

        goal = FindShortestPAthAStar(startNode, goalNode, graph);
        if (goal == startNode || !nodeParents.ContainsKey(nodeParents[goal]))
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
        uint nodeVisitCount = 0;
        float timeNow = Time.realtimeSinceStartup;

        IDictionary<GraphNode, int> heuristicScore = new Dictionary<GraphNode, int>();

        IDictionary<GraphNode, int> distanceFromStart = new Dictionary<GraphNode, int>();

        foreach (GraphNode node in graph.Nodes)
        {
            heuristicScore.Add(new KeyValuePair<GraphNode, int>(node, int.MaxValue));
            distanceFromStart.Add(new KeyValuePair<GraphNode, int>(node, int.MaxValue));
        }

        heuristicScore[start] = EuclideanEstimate(start.transform.position, goal.transform.position);
        distanceFromStart[start] = 0;

        PriorityQueue<int, GraphNode> priorityQueue = new PriorityQueue<int, GraphNode>();
        priorityQueue.Enqueue(heuristicScore[start], start);
        while (priorityQueue.Count > 0)
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
