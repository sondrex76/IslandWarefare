using UnityEngine;

// Resource gathering building template
public class AbstractResourceHarvesting : AbstractBuilding
{
    bool resourceFound = false;                                     // Specifies if a resource have been found
    Resource resource;                                              // Resource currently being utilized
    GameManager gameManager;                                        // The game manager object

    [SerializeField] Resource.ResourceTypes[] neededResource;       // Valid resource types for building
    [SerializeField] float resourceExtractionSpeed;                 // Speed of resource extraction(fixed update, is run 50x per second)

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();              // Finds game manager
    }

    // Code to be run on fixedUpdate
    override protected void BuildingFunctionality()
    {
        if (resourceFound)                                          // If there is a valid resource identified
        {
            switch (resource.ReturnType())                          // Depending on which element a different value from GameManager is to be updated
            {
                case Resource.ResourceTypes.FOOD:
                    gameManager.resourceFood += UpdateResource();
                    break;
                case Resource.ResourceTypes.WOOD:
                    gameManager.resourceWood += UpdateResource();
                    break;
                case Resource.ResourceTypes.IRON:
                    gameManager.resourceIron += UpdateResource();
                    break;
                case Resource.ResourceTypes.COPPER:
                    gameManager.resourceCopper += UpdateResource();
                    break;
                case Resource.ResourceTypes.URANIUM:
                    gameManager.resourceUranium += UpdateResource();
                    break;
                case Resource.ResourceTypes.COAL:
                    gameManager.resourceCoal += UpdateResource();
                    break;
                case Resource.ResourceTypes.ALUMINIUM:
                    gameManager.resourceAluminium += UpdateResource();
                    break;
                case Resource.ResourceTypes.GOLD:
                    gameManager.resourceGold += UpdateResource();
                    break;
                case Resource.ResourceTypes.OIL:
                    gameManager.resourceOil += UpdateResource();
                    break;
                case Resource.ResourceTypes.ANIMALS:
                    gameManager.resourceAnimals += UpdateResource();
                    break;
            }
        }
    }

    // Updates resource and ensures resource value does not go below 0
    float UpdateResource()
    {
        float returnedValue;                                        // Value to return

        if (resource.resourceAmount - resourceExtractionSpeed <= 0) // if resource becomes empty
        {
            returnedValue = resource.resourceAmount;                // Sets amount returned to the remaining resources
            resourceFound = false;                                  // Updates status to showcase that a resource is not currently located
            try // Tries to allow usage of Destro yrather then DestroyImmidiate
            {
                Destroy(resource);                             // Destroys resource class
                Destroy(resource.gameObject);                  // Destroys resource gameobject
            }
            catch (MissingReferenceException e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            resource.resourceAmount -= resourceExtractionSpeed;     // Updates amount of resource
            returnedValue = resourceExtractionSpeed;                // Makes the amount to increase resource the extraction speed
        }

        return returnedValue;
    }

    // checks if resource is there
    private void OnTriggerStay(Collider other)
    {
        if (!resourceFound && other.tag == "Resource")                              // Checks if it is a resource, but only if a resource have not already been found
        {
            // Goes through all valid resources
            foreach (Resource.ResourceTypes type in neededResource)                 // Goes through all valid resources
            {
                if (other.gameObject.GetComponent<Resource>().ReturnType() == type) // If the resource is of the correct type
                {
                    resource = other.gameObject.GetComponent<Resource>();
                    resourceFound = true;
                }
            }
        }
    }
}
