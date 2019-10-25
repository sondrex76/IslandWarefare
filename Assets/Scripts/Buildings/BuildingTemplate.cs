using UnityEngine;

/* Template for buildings
 * To make a building a model for the building must be added to the game object
*/
public class BuildingTemplate : MonoBehaviour
{
    GameManager gameManager;                // Game manager object, is used in child objects to modify resource amounts

    [SerializeField] float maxHealth;       // Max health for building
    [SerializeField] float currentHealth;   // Current health

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Updates 50 times a second independent of framerate
    private void FixedUpdate()
    {
        // Runs the building's primary functionality
        BuildingFunctionality();
    }

    // How much health should the building take, negative value for healing
    public bool HurtBuilding(float lostHealth)  // Returns true if building is destroyed
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
    void BuildingFunctionality()
    {
        // Fill in in child, this code wil lvary from building to building
    }

    // What do do when building reaches 0 hp, overwrite in child class for more complex behaviour
    void ZeroHealth()
    {
        DestroyBuilding();  // Destroys building
    }

    // Destroys this building
    void DestroyBuilding()
    {
        Destroy(this);
    }
}
