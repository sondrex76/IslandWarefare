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
    bool firstFrame;

    // Start is called before the first frame update
    void Start()
    {
        firstFrame = false;

    }

    private void Reset()
    {
        Nodes = GetComponentsInChildren<GraphNode>(true).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstFrame) 
        {
            firstFrame = true;
            
            _eventManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<EventManager>();
            var f = GameObject.FindGameObjectsWithTag("Player");
            foreach (var g in f)
            {
                _eventManager._listenToFlag.AddListener(g.GetComponent<DestinationList>().agentDestination);
            }
            
            var r = new System.Random();
            foreach (int i in Enumerable.Range(0, Nodes.Count).OrderBy(x => r.Next()))
            {
                _eventManager._listenToFlag.Invoke(Nodes[i].transform.position, true);
                
            }
            foreach (var g in f)
            {
                _eventManager._listenToFlag.RemoveListener(g.GetComponent<DestinationList>().agentDestination);
            }
            
        }
        

    }
}