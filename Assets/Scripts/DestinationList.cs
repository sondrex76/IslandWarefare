using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

public class DestinationList : MonoBehaviour
{
    
    NavMeshAgent _agent;
    List<Vector3> _listOfDestinations;
    bool _AgentHasReached = true;
    Vector3 _destination;
    Graph _graph;
    GraphNode _currentNode;
    bool test;
    
    // Start is called before the first frame update
    void Start()
    {
        test = true;
        _graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        _agent = GetComponent<NavMeshAgent>();
        _destination = new Vector3();
        _listOfDestinations = new List<Vector3>();
        _currentNode = traverseNodesRandomly();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_AgentHasReached)
        {
            if (_listOfDestinations.Count > 0 && !test)
            {
                _destination = _listOfDestinations[0];
                _listOfDestinations.RemoveAt(0);
                _agent.destination = _destination;
                _AgentHasReached = false;
            }
            if (test) {
                
                var r = new System.Random();
                _currentNode = _currentNode.Adjacent[r.Next(0, _currentNode.Adjacent.Count)];
                _destination = _currentNode.transform.position;
                _agent.destination = _destination;
                _AgentHasReached = false;
            }
        }
        if (Vector3.Distance(transform.position, _destination) < 1.0f)
        {
            _AgentHasReached = true;
        }
        
    }


    public void agentDestination(Vector3 arg, bool boolArg) 
    {
        Debug.Log(arg);
        if (boolArg)
        {
            _listOfDestinations.Add(arg);
        } else {
            _listOfDestinations.Clear();
            _destination = arg;
            _agent.destination = _destination;
            _AgentHasReached = false;
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
