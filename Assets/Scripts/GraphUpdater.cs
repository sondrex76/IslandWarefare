using UnityEngine;

[ExecuteInEditMode]
public class GraphUpdater : MonoBehaviour
{
    [SerializeField]
    private Graph graph;

    [SerializeField]
    private int timesliceInterval;

    [SerializeField]
    private int currentFrame;

    // Start is called before the first frame update
    void Start()
    {
        if (graph == null)
            graph = GetComponent<Graph>();

        currentFrame = 0;

        // timeslices should happen every other frame
        if (timesliceInterval <= 0)
            timesliceInterval = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (graph == null)
            return;

        currentFrame++;

        // Wait for the frame count to surpass the timeslice interval, so we do not execute the update every frame
        if (currentFrame < timesliceInterval)
            return;
        else
            currentFrame = 0;

        // loop through all graph nodes
        foreach (var node in graph.Nodes)
        {
            // look through the adjacent nodes
            foreach(var adjacentNode in node.Adjacent)
            {
                if (node == adjacentNode)
                    continue;

                if (graph.Edges == null)
                    continue;

                // find an existing directional edge that is cached in the graph
                var existingEdge = graph.Edges.Find(edge => edge.EndNode == adjacentNode
                   && edge.StartNode == node);

                // create the directional edge if its not cached
                if(existingEdge == null)
                {
                    var edge = new Graph.Edge();
                    edge.StartNode = node;
                    edge.EndNode = adjacentNode;
                    edge.Weight = 1;

                    graph.Edges.Add(edge);
                }
            }
        }
    }
}
