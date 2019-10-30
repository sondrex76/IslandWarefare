using UnityEngine;

// Template for buildings
// To make a building a model for the building must be added to the game object
// 

public class AbstractBuilding : MonoBehaviour
{
    protected GameManager gameManager;                // Game manager object, is used in child objects to modify resource amounts
    [SerializeField] GameObject prefab;     // Prefab object

    [SerializeField] float maxHealth;       // Max health for building
    [SerializeField] float currentHealth;   // Current health

    protected void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Updates 50 times a second independent of framerate
    protected void FixedUpdate()
    {
        // Runs the building's primary functionality
        BuildingFunctionality();
    }


    // How much health should the building take, negative value for healing
    protected virtual bool HurtBuilding(float lostHealth)  // Returns true if building is destroyed
    {
        currentHealth -= lostHealth;

        // If building has been destroyed
        if (currentHealth <= 0)
        {
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
        DestroyBuilding();  // Destroys building
    }

    // Destroys this building, you can send it the coordinates 
    protected virtual void DestroyBuilding(float x = 0, float y = 0, float z = 0, float rotation = 0)
    {
        // By default the building will place a prefab at the relative coordinates of (0, 0, 0), will not happen if prefab is not defined
        if (prefab != null)
        {
            LoadPrefab(prefab, new Vector3(x, y, z), rotation);
        }
        Destroy(this);
    }

    // Loads a prefab, at relative x, y, z coordinates rotated around the y axis by rotation comapred to building
    // Is used with a check to see if the prefab is null first: if (prefab != null) {}
    protected virtual void LoadPrefab(GameObject gameObject, Vector3 coordinates, float rotation)
    {
        GameObject newObject = Instantiate(gameObject, coordinates, Quaternion.identity);   // Places object
        newObject.transform.Rotate(0, rotation, 0);                                         // Rotates object
    }
}
