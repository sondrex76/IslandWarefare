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

    // Function to run when loading a resource from a save which updates the amount of the resource present

    public void LoadFromSave(float amount)
    {
        resourceAmount = amount;
    }

    public ResourceSave ReturnResourceSave(Vector3 position, Vector3 rotation)
    {
        // Debug.Log(resourceType.transform.name);
        return new ResourceSave(resourceAmount, resourceType.transform.name, position, rotation);
    }
}
