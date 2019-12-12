using UnityEngine;

// Template for buildings
// To make a building a model for the building must be added to the game object
// 

public class AbstractBuilding : MonoBehaviour
{
    protected GameManager gameManager;              // Game manager object, is used in child objects to modify resource amounts
    [SerializeField] GameObject prefab;             // Prefab object, usually ruins to be placed after reaching 0hp
    [SerializeField] protected Transform building;  // Transform of current building

    [SerializeField] float maxHealth;               // Max health for building
    [SerializeField] float startHealth;             // Starting health
    [SerializeField] float startOffsetY = 20;       // How far below the surface the building starts
    [SerializeField] float timeSecondsBuild;        // Time in seconds for how long it will take ofr the building to finish building
    [SerializeField] float randomFluct = 0.1f;      // Max fluctuation from zero for building

    protected bool finishedBuilding = false;        // Bool specifying if building is finished being built
    [SerializeField] float currentHealth;           // Current health, serialized for convenience' sake but works automatically
    public Sprite clickableIcon;                    // Icon to show a user when selecting a building

    [SerializeField] protected GameObject node;


    // protected bool test = false;
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
            building.localPosition = new Vector3(Random.Range(-randomFluct, randomFluct), building.transform.localPosition.y - startOffsetY, Random.Range(-randomFluct, randomFluct));
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
                building.localPosition = new Vector3(Random.Range(-randomFluct, randomFluct), building.localPosition.y + (startOffsetY / 50 / timeSecondsBuild), Random.Range(-randomFluct, randomFluct));
                building.localPosition = Vector3.Scale(building.localPosition, transform.up);

                // Updates health so that it becomes full by the time the building is finished building
                HurtBuilding(-(1.0f / 50 / timeSecondsBuild) * (maxHealth - startHealth));

                if (building.localPosition.y >= 0) // Building is finished building
                {
                    // Increases number of houses present
                    GameManager.numHouses++;    
                    // Sets building to finished building
                    finishedBuilding = true;    
                    // Limits max helath to maxHealth
                    if (currentHealth > maxHealth)
                        currentHealth = maxHealth; 
                    // Sets position to base position
                    building.localPosition = new Vector3(0, 0, 0);
                    
                    // Node
                    MakeNode();
                }
            }
        }
    }

    // Runs code to generate node
    protected virtual void MakeNode()
    {
        // Move code below
        ObjectPool pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        GameObject graph = GameObject.FindGameObjectWithTag("Graph");
        node = pool.GetPooledObject("AbstractNode");
        
        node.transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), transform.position.z);
        node.transform.parent = graph.transform;
        node.SetActive(true); 
        graph.GetComponent<Graph>().AddNodes();
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
        DestroyBuilding(transform.rotation.eulerAngles, transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);  // Destroys building
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

    // Loads everything except coordinates based on variables sent in it, not meant to be overwritten
    // Factory must also run LoadFactory(<variables>)
    public virtual void LoadFromSave(float presentHealth, bool buildingFinished, float yOffset)
    {
        // Updates bool
        finishedBuilding = buildingFinished;

        // Checks if building is finished building
        if (finishedBuilding)
        {
            // sets position to 0, next fixedUpdate will set the building to a finished state
            building.localPosition = new Vector3(0, 0, 0);
            MakeNode();                 // runs node related code
            GameManager.numHouses++;    // Increases number of houses
        }
        else
        {
            // Sets coordinates to position while building
            building.localPosition = new Vector3(Random.Range(-randomFluct, randomFluct), yOffset, Random.Range(-randomFluct, randomFluct));
        }
        // Updates health
        currentHealth = presentHealth;
    }

    // Returns datavalues of class for use in saving
    public BuildingSave ReturnBuildingSave(Vector3 position, Vector3 rotation)
    {
        return new BuildingSave(currentHealth, startOffsetY, finishedBuilding, transform.name, position, rotation);
    }
}
