using UnityEngine;

// Template for buildings
// To make a building a model for the building must be added to the game object
// 

public class AbstractBuilding : MonoBehaviour
{
    protected GameManager gameManager;          // Game manager object, is used in child objects to modify resource amounts
    [SerializeField] GameObject prefab;         // Prefab object, usually ruins to be placed after reaching 0hp
    [SerializeField] Transform building;        // Transform of current building

    [SerializeField] float maxHealth;           // Max health for building
    [SerializeField] float startHealth;         // Starting health
    [SerializeField] float startOffsetY = 20;   // How far below the surface the building starts
    [SerializeField] float timeSecondsBuild;    // Time in seconds for how long it will take ofr the building to finish building
    [SerializeField] float randomFluct = 0.1f;  // Max fluctuation from zero for building

    bool finishedBuilding= false;               // Bool specifying if building is finished being built
    [SerializeField] float currentHealth;       // Current health, serialized for convenience' sake but works automatically

    protected void Awake()
    {
        // Sets health to starting health
        currentHealth = startHealth;            
        gameManager = FindObjectOfType<GameManager>();

        // If building is not defined it searches for object called "Building"
        if (building == null)               
            building = transform.Find("Building").GetComponent<Transform>();

        // Sets position to the appropriate starting position
        if (building != null)               // DEBUG, in case object has no model, should not be needed later
            building.position = new Vector3(0, building.transform.position.y - startOffsetY, 0);
    }

    // Updates 50 times a second independent of framerate
    protected void FixedUpdate()
    {
        if (!GameManager.isPaused)  // Only run code if game is not paused
        {
            // Either runs bulding's funcitonality or builds it
            if (finishedBuilding)               // Building is up and running
            {
                // Runs the building's primary functionality
                BuildingFunctionality();
            }
            else                                // Building is being built
            {
                // Updates postion by one 50th of startOffsetY / timeSecondsBuild to make the time it takes to reach ideal position to be timeSecondsBuild
                building.position = new Vector3(Random.Range(-randomFluct, randomFluct), building.transform.position.y + (startOffsetY / 50 / timeSecondsBuild), Random.Range(-randomFluct, randomFluct));

                // Updates health so that it becomes full by the time the building is finished building
                HurtBuilding(-(1.0f / 50 / timeSecondsBuild) * (maxHealth - startHealth));

                if (building.position.y >= 0) // Building is finished building
                {
                    // Sets building to finished building
                    finishedBuilding = true;    
                    // Limits max helath to maxHealth
                    if (currentHealth > maxHealth)
                        currentHealth = maxHealth; 
                    // Sets position to base position
                    building.position = new Vector3(0, 0, 0);
                }
            }
        }
    }

    // How much health should the building take, negative value for healing
    protected virtual bool HurtBuilding(float lostHealth)  // Returns true if building is destroyed
    {
        currentHealth -= lostHealth;

        // If building has been destroyed
        if (currentHealth <= 0)
        {
            Debug.Log("ZERO");  // DEBUG
            ZeroHealth();
            return true;
        }

        return false;
    }

    // The primary functionality run every fixed update
    protected virtual void BuildingFunctionality()
    {
        // Fill in in child, this code will vary from building to building
    }

    // What do do when building reaches 0 hp, overwrite in child class for more complex behaviour
    protected virtual void ZeroHealth()
    {
        DestroyBuilding(transform.rotation.eulerAngles, transform.position.x, transform.position.y, transform.position.z);  // Destroys building
    }

    // Destroys this building, you can send it the coordinates 
    protected virtual void DestroyBuilding(Vector3 rotation, float x = 0, float y = 0, float z = 0)
    {
        // By default the building will place a prefab at the relative coordinates of (0, 0, 0), will not happen if prefab is not defined
        // USed to place ruins
        if (prefab != null)
        {
            // TODO: Make rotation of loaded object equal that of building being deleted
            LoadPrefab(prefab, new Vector3(x, y, z), rotation);
        }
        Destroy(gameObject);
    }

    // Loads a prefab, at relative x, y, z coordinates rotated around the y axis by rotation comapred to building
    // Is used with a check to see if the prefab is null first: if (prefab != null) {}
    protected virtual void LoadPrefab(GameObject gameObject, Vector3 coordinates, Vector3 rotation)
    {
        GameObject newObject = Instantiate(gameObject, coordinates, Quaternion.Euler(rotation));   // Places object
    }

    // Function defining the building as finished beng built, used when loading level
    public virtual void IsFinished(float health)
    {
        // sets position to 0, next fixedUpdate will set the building to a finished state
        building.position = new Vector3(0, 0, 0);

        // Updates health
        currentHealth = health;
    }
}
