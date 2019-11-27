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
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
        for (int i = 0; i < Nodes.Count; i++) {
            Nodes[i].Adjacent.Clear();
            for (int j = 0; j < Nodes.Count; j++) {
                if (i != j)
                {
                    if(Vector3.Distance(Nodes[i].transform.position, Nodes[j].transform.position) < 6.0f) {
                        Nodes[i].Adjacent.Add(Nodes[j]);
                    }
                    
                }
            }
            Debug.Log(Nodes[i].Adjacent.Count);
        }

    }

    private void Reset()
    {
         Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
        for (int i = 0; i < Nodes.Count; i++) {
            Nodes[i].Adjacent.Clear();
            for (int j = 0; j < Nodes.Count; j++) {
                if (i != j)
                {
                    if(Vector3.Distance(Nodes[i].transform.position, Nodes[j].transform.position) < 6.0f) {
                        Nodes[i].Adjacent.Add(Nodes[j]);
                    }
                    
                }
            }
            Debug.Log(Nodes[i].Adjacent.Count);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}