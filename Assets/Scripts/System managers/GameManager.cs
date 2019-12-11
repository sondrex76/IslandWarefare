using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static InputManager inputManager;        // The input manager
    public static bool isPaused = false;            // Is paused

    // Array of resources
    public static Resource.ResourceAmount[] resources;

    // Miltary mght, might be expanded upon later
    public static float defensivePower = 0;
    public static float offensivePower = 0;
    public static float supplyPower = 0;

    // Amount of resources generated through various systems
    public static float moneyAmount = 0;            // Money
    public static uint population = 0;              // Population
    public static float happiness = 0;              // Happiness, might be changed to be a value between 0 and 100 in the future

    // Bools for states
    public static bool isInGUI;                     // Specifies that the user is in a GUI and it should not be shut down

    [SerializeField] Image arrow;                   // Arrow to be placed above buildings


    float previousTimeSpeed = 1;                    // Previous speed of time

    // Options
    [SerializeField] Canvas optionsMenu;            // The options menu
    [SerializeField] OptionsManager optionsManager; // The options manager

    FactoryBuilding currentlySelectedFactory;       // Currently selected factory
    bool factorySelected = false;

    // Start is called before the first frame update
    void Awake()
    {
        // DEBUG, limit framerate
        QualitySettings.vSyncCount = 1;

        // Loads all resources automatically
        GameObject[] resourceObjects = Resources.LoadAll("Prefabs/WorldResources").Cast<GameObject>().ToArray();
        
        // Loads resources into static with 0 as the amount in all cases
        resources = new Resource.ResourceAmount[resourceObjects.Length];
        for (int i = 0; i < resourceObjects.Length; i++)
        {
            Resource currentResourceObject = resourceObjects[i].GetComponent<Resource>();
            Resource.ResourceAmount currentResource;
            currentResource.amount = 100;
            currentResource.resource = currentResourceObject;

            // Defines current resource
            resources[i] = currentResource;
        }
        
        // DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        optionsMenu.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Start()
    {
        try
        {
            GameObject.Find("Terrain").layer = LayerMask.NameToLayer("Ground");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Update()
    {
        // If the game is not paused
        if (!isPaused)
        {

            // Checks if primary mouse button is down
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                
                // Factory
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Factory")
                {
                    if (currentlySelectedFactory != null)                                           // Checks if a building is already selected
                    {
                        currentlySelectedFactory.ActivateGUI(false);                                // Disables menu of previously selected game object
                    }
                    currentlySelectedFactory = hitInfo.transform.GetComponent<FactoryBuilding>();   // Sets newly selected game object
                    
                    // Makes sure it only trggers if FactoryBuilding were found
                    if (currentlySelectedFactory != null)
                    {
                        factorySelected = true;                                                     // Updates if factory is enabled
                        currentlySelectedFactory.ActivateGUI(true);                                 // Activates GUI of game object

                        // Sets arrow position above currently selected building
                        arrow.transform.position = new Vector3(currentlySelectedFactory.transform.position.x, currentlySelectedFactory.transform.position.y + 45, currentlySelectedFactory.transform.position.z);
                        arrow.enabled = true;
                    }
                }
                else if (currentlySelectedFactory != null && !isInGUI)                              // Checks if there is a factory there and that you are outside of any relevant GUI element
                {
                    factorySelected = false;
                    arrow.enabled = false;
                    currentlySelectedFactory.ActivateGUI(false);                                    // Disables menu of previously selected game object
                }
                else
                {
                    factorySelected = false;
                }
            }

            if (factorySelected) // checks if the user is in a relevant GUI(Factory)
            {
                // Make the arrow always face the player while active
                arrow.transform.LookAt(new Vector3(Camera.main.transform.position.x, currentlySelectedFactory.transform.position.y, Camera.main.transform.position.z));
            }
        }
    }
    

    public void TrggerCanvas()
    {
        UpdateCanvas(!isPaused);
    }

    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        optionsMenu.enabled = isPaused = active;

        // if paused then it sets speed to zero
        if (active)
        {
            previousTimeSpeed = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = previousTimeSpeed;
        }

        // Checks to see if the camera exists in case we are on the pause menu
        if (Camera.main.transform.GetComponent<Rigidbody>() != null)
        {
            Camera.main.transform.GetComponent<Rigidbody>().freezeRotation = !active;

            // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
            Cursor.visible = active || inputManager.frozenAngle; // WIP
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
        optionsManager.currentButton = EventSystem.current.currentSelectedGameObject;
    }

    // Turns on/off rendering of resources
    public void ResourceRendering(bool render)
    {
        if (render)
        {
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Resources");
        }
        else
        {
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Resources"));
        }
    }

    // Toggle rendering of resources
    public void ToggleResourceRendering()
    {
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer("Resources");
    }

    // Quits application
    public void QuitGame()
    {
        Application.Quit();
    }
}
