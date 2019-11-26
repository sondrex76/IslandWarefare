using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        _agent = GetComponent<NavMeshAgent>();
        _destination = new Vector3();
        _currentNode = traverseNodesRandomly();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_AgentHasReached)
        {
            
            var r = new System.Random();
            _currentNode = _currentNode.Adjacent[r.Next(0, _currentNode.Adjacent.Count)];
            _destination = _currentNode.transform.position;
            _agent.destination = _destination;
            _AgentHasReached = false;
            
        }
        
        if (Vector3.SqrMagnitude(transform.position - _destination) <= 1.0f)
        {
            _AgentHasReached = true;
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
}
