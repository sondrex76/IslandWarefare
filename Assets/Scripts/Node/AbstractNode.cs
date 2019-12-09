using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractNode : MonoBehaviour
{
    [Range(2, 100)]
    public int _capacity;
    public List<GameObject> _citizens;

    protected virtual void Start()
    {
       _citizens = new List<GameObject>();
    }

    public void AddCitizen(GameObject citizen)
    {
        _citizens.Add(citizen);
    }

    public void RemoveCitizen(GameObject citizen)
    {
        _citizens.Remove(citizen);
    }

    public bool OverCapacity()
    {
        return _citizens.Count >= _capacity;
    }
}
