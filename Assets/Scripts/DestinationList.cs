using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class DestinationList : MonoBehaviour
{
    
    NavMeshAgent _agent;
    
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void agentDestination(Vector3 arg, bool boolArg) 
    {
        _agent.destination = arg;
    }

}
