using UnityEngine;

// save for resource objects
[System.Serializable]
public class ResourceSave : WorldObjectSave
{
    public float resourceAmount;
    public string resourceName;

    public ResourceSave(float amount, string name, Vector3 position, Vector3 rotation) : base(position, rotation)
    {
        resourceAmount = amount;
        resourceName = name;
    }
}
