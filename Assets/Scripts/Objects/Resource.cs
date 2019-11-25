using UnityEngine;

// Class for resource objects
[System.Serializable]
public class Resource : MonoBehaviour
{
    [System.Serializable]
    public struct ParentResourceAmounts
    {
        [SerializeField] float amount;
        [SerializeField] Resource resource;
    };

    [SerializeField]
    ParentResourceAmounts[] parentResources;

    // Resource properties
    [SerializeField] float sellingPrice;        // Price the resource can be sold for
    [SerializeField] string resourceName;       // Name of resource
    [SerializeField] float productionTimeSec;   // Production time in seconds

    // Returns parents of resource
    public ParentResourceAmounts[] ReturnParents()
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
}
