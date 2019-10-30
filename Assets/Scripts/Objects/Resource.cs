using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum ResourceTypes{ FOOD, WOOD, IRON, COPPER, URANIUM, COAL, ALUMINIUM, GOLD, OIL, ANIMALS }

    [SerializeField] ResourceTypes resourceType;    // Type of resource
    public float resourceAmount;                    // Amount of resource

    // Returns the resource's resource type
    public ResourceTypes ReturnType()
    {
        return resourceType;
    }
}
