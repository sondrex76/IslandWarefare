using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommercialManager : MonoBehaviour
{
    [Range(0, 20)]
    public int _shopperCapacity;
    List<GameObject> _shopper;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddShopper(GameObject citizen)
    {
        _shopper.Add(citizen);
    }

    public void RemoveShopper(GameObject citizen)
    {
        _shopper.Remove(citizen);
    }

    public bool ShopperOverCapacity()
    {
        return _shopper.Count < _shopperCapacity;
    }
}
