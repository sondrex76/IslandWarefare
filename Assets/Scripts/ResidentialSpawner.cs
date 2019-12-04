﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(2, 10)]
    public int _capacity;
    public List<GameObject> _citizenLiving;
    ObjectPool _pool;
    [SerializeField]
    Graph _graph;
    void Start()
    {
        _pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        for (int i = 0; i < _capacity; i++)
        {
            GameObject temp = _pool.GetPooledObject("Citizen");
            temp.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            temp.GetComponent<CitizenDestinationManager>().SetHome(this.GetComponent<GraphNode>());
            temp.SetActive(true);
            _citizenLiving.Add(temp);
        }
        _graph.GetComponent<Graph>().AddNodes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
