using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MilitaryBuilding : FactoryBuilding
{

    [SerializeField] GameObject gui;                    // GUI element

    [SerializeField]
    MilitaryUnit[] units;



    // Start is called before the first frame update
    void Start()
    {
        if (gui == null)                    // Checks if GUI have already been set
            gui = GameObject.Find("GUI");   // Sets GUI

        ActivateGUI(false);
    }

    //Places a node that the AI can use to traverse
    protected override void MakeNode()
    {
        ObjectPool pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        GameObject graph = GameObject.FindGameObjectWithTag("Graph");
        node = pool.GetPooledObject("OfficeNode");

        node.transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), transform.position.z);
        node.transform.parent = graph.transform;
        node.SetActive(true);
        graph.GetComponent<Graph>().AddNodes();
    }
}
