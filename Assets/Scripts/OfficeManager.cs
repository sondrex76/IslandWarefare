using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    [Range(0, 20)]
    public int _workerCapacity;
    List<GameObject> _workers;
    void Start()
    {
        _workers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWorker(GameObject citizen)
    {
        _workers.Add(citizen);
    }

    public bool WorkersOverCapacity()
    {
        return _workers.Count < _workerCapacity;
    }
}
