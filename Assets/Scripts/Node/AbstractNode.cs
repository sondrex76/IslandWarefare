using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractNode : MonoBehaviour
{
    [Range(2, 100)]
    public int capacity;
    public List<GameObject> citizens;

    protected virtual void Start()
    {
       citizens = new List<GameObject>();
    }

    public void AddCitizen(GameObject citizen)
    {
        citizens.Add(citizen);
    }

    public void RemoveCitizen(GameObject citizen)
    {
        citizens.Remove(citizen);
    }

    public bool OverCapacity()
    {
        return citizens.Count >= capacity;
    }
}
