using UnityEngine;

// Class for resource objects
[System.Serializable]
public class Resource : MonoBehaviour
{
    // Resource parent properties
    [SerializeField] float[] parentAmount;
    [SerializeField] Resource[] parentResource;

    // Resource properties
    [SerializeField] float sellingPrice;        // Price the resource can be sold for
    [SerializeField] string resourceName;       // Name of resource
    [SerializeField] float productionTimeSec;   // Production time in seconds

    // Returns parents of resource
    public Resource[] ReturnParents()
    {
        return parentResource;
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
}
