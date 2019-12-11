using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.AI;

public class DestinationList : MonoBehaviour
{
    
    NavMeshAgent agent;
    List<Vector3> listOfDestinations;
    bool AgentHasReached = true;
    Vector3 destination;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = new Vector3();
        listOfDestinations = new List<Vector3>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AgentHasReached)
        {
            if (listOfDestinations.Count > 0)
            {
                destination = listOfDestinations[0];
                listOfDestinations.RemoveAt(0);
                agent.destination = destination;
                AgentHasReached = false;
            }
        }
        if (Vector3.SqrMagnitude(transform.position - destination) <= 1.0f)
        {
            AgentHasReached = true;
        }
        
    }


    public void agentDestination(Vector3 arg, bool boolArg) 
    {
        Debug.Log(arg);
        if (boolArg)
        {
            listOfDestinations.Add(arg);
        } else {
            listOfDestinations.Clear();
            destination = arg;
            agent.destination = destination;
            AgentHasReached = false;
        }
    }
}
