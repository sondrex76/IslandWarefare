using UnityEngine;

public class ResourceWorldObject : MonoBehaviour
{
    [SerializeField] Resource resourceType;    // Type of resource
    public float resourceAmount;                    // Amount of resource

    // Returns the resource's resource type
    public Resource ReturnType()
    {
        return resourceType;
    }
}
