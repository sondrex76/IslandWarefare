using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class DestinationList : MonoBehaviour
{
    
    NavMeshAgent _agent;
    List<Vector3> _listOfDestinations;
    bool _AgentHasReached = true;
    Vector3 _destination;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _destination = new Vector3();
        _listOfDestinations = new List<Vector3>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_AgentHasReached)
        {
            if (_listOfDestinations.Count > 0)
            {
            _destination = _listOfDestinations[0];
            _listOfDestinations.RemoveAt(0);
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

}
