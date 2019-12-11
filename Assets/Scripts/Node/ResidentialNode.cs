using System.Collections.Generic;
using UnityEngine;

public class ResidentialNode : AbstractNode
{
    override protected void Start()
    {
        citizens = new List<GameObject>();
        
        ObjectPool pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        for (int i = 0; i < capacity; i++)
        {
            GameObject temp = pool.GetPooledObject("Simple Citizen");

            temp.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            temp.GetComponent<CitizenDestinationManager>().SetHome(this.GetComponent<GraphNode>());
            temp.transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
            temp.SetActive(true);
            citizens.Add(temp);
        }
    }

}
