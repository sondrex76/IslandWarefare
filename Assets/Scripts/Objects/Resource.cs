using UnityEngine;

// Class for resource objects
[System.Serializable]
public class Resource : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceAmount
    {
        public float amount;
        public Resource resource;
    };

    [SerializeField]
    ResourceAmount[] parentResources;

    // Resource properties
    [SerializeField] float sellingPrice;        // Price the resource can be sold for
    [SerializeField] string resourceName;       // Name of resource
    [SerializeField] float productionTimeSec;   // Production time in seconds

    // Returns parents of resource
    public ResourceAmount[] ReturnParents()
    {
        return parentResources;
    }

    // Returns resource price
    public float ReturnResourcePrice()
    {
        return sellingPrice;
    }

    // Returns resource name
    public string ReturnResourceName()
    {
        return resourceName;
    }

    // Returns time to produce in seconds
    public float ReturnProductionTime()
    {
        return productionTimeSec;
    }
}
