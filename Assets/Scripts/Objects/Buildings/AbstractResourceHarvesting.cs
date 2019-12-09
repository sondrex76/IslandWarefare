using UnityEngine;

// Resource gathering building template
public class AbstractResourceHarvesting : AbstractBuilding
{
    bool resourceFound = false;                                         // Specifies if a resource have been found
    int resourceIndex = 0;                                              // Indext of current resource in GameManager
    ResourceWorldObject resource;                                       // Resource currently being utilized

    [SerializeField] Resource[] neededResource;                         // Valid resource types for building
    [SerializeField] float resourceExtractionSpeed;                     // Speed of resource extraction(fixed update, is run 50x per second)

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();                  // Finds game manager
    }

    // Code to be run on fixedUpdate
    override protected void BuildingFunctionality()
    {
        if (resourceFound)                                              // If there is a valid resource identified
        {
            // Updates resource amount
            GameManager.resources[resourceIndex].amount += UpdateResource();
        }
    }

    // Updates resource and ensures resource value does not go below 0
    float UpdateResource()
    {
        if (!GameManager.isPaused)
        {
            float returnedValue;                                        // Value to return

            if (resource.resourceAmount - resourceExtractionSpeed <= 0) // if resource becomes empty
            {
                returnedValue = resource.resourceAmount;                // Sets amount returned to the remaining resources
                resourceFound = false;                                  // Updates status to showcase that a resource is not currently located

                if (resource != null)                                   // Checks if resource is null since Destroy will trigger at the end of fixed update
                {
                    Destroy(resource);                                  // Destroys resource class
                    Destroy(resource.gameObject);                       // Destroys resource gameobject
                }
            }
            else
            {
                returnedValue = resourceExtractionSpeed;                // Makes the amount to increase resource the extraction speed
            }

            resource.resourceAmount -= returnedValue;                   // Updates amount of resource

            return returnedValue;
        }
        else return 0;
    }

    // checks if resource is there
    private void OnTriggerStay(Collider other)
    {

        if (!resourceFound && other.tag == "Resource")                  // Checks if it is a resource, but only if a resource have not already been found
        {
            // Goes through all valid resources
            foreach (Resource type in neededResource)                   // Goes through all valid resources
            {    
                // Checks if the type of the resource is correct
                if (other.gameObject.GetComponentInChildren<ResourceWorldObject>().ReturnType().ToString() == type.ToString())
                {
                    resource = other.gameObject.GetComponent<ResourceWorldObject>();
                    resourceFound = true;

                    // Goes through resources of GameManager and finds which one is being harvested, before setting the resource index to that resource's idnex
                    for (int i = 0; i < GameManager.resources.Length; i++)
                    {
                        if (GameManager.resources[i].resource == resource.ReturnType())
                        {
                            resourceIndex = i;
                        }
                    }
                }
            }
        }
    }
}
