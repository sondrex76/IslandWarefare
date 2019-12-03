using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{
    [System.Serializable]
    public class Edge
    {
        public GraphNode StartNode;

        public GraphNode EndNode;

        [Range(0f, 10f)]
        public int Weight;
    }

    

    public List<GraphNode> Nodes;
    public List<Edge> Edges; 
    EventManager _eventManager;

    // Start is called before the first frame update
    void Start()
    {
        AddNodes();

    }

    private void Reset()
    {
        AddNodes();
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void AddNodes()
    {
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
        for (int i = 0; i < Nodes.Count; i++) {
            Nodes[i].Adjacent.Clear();
            for (int j = 0; j < Nodes.Count; j++) {
                if (i != j)
                {
                    switch(Nodes[i]._attribute)
                    {
                        case GraphNode.Attribute.Road : 
                            if(Vector3.Distance(Nodes[i].transform.position, Nodes[j].transform.position) < 5.0f) {
                                Nodes[i].Adjacent.Add(Nodes[j]);
                            }
                            break;
                        default :
                            if(Vector3.Distance(Nodes[i].transform.position, Nodes[j].transform.position) < 10.0f) {
                                Nodes[i].Adjacent.Add(Nodes[j]);
                            }
                            break;
                    }
                    
                    
                }
            }
        }
        // Finds all non road nodes
        // Thne it finds those nodes adjacent nodes that are roads
        // And add the noode to the adjacent nodes of the road nodes
        foreach(var x in Nodes.Where(i => i._attribute != GraphNode.Attribute.Road))
        {
            foreach(var y in x.Adjacent.Where(j => j._attribute == GraphNode.Attribute.Road))
            {
                y.Adjacent.Add(x);
            }
        }
    }
}