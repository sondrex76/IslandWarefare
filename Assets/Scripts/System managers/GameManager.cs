using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static InputManager inputManager;        // The input manager
    public static bool isPaused = false;            // Is paused

    // Resources, resources[i] and resourceAmounts[i] are for object i
    public Resource[] resources;
    public float[] resourceAmounts;
    // Miltary mght, might be expanded upon later
    public float defensivePower = 0;
    public float offensivePower = 0;
    public float supplyPower = 0;

    // Amount of resources generated through various systems
    public float moneyAmount = 0;                   // Money
    public uint population = 0;                     // Population
    public float happiness = 0;                     // Happiness, might be changed to be a value between 0 and 100 in the future

    float previousTimeSpeed = 1;                    // Previous speed of time

    // Options
    [SerializeField] Canvas optionsMenu;            // The options menu
    [SerializeField] OptionsManager optionsManager; // The options manager

    FactoryBuilding currentlySelectedFactory;       // Currently selected factory



    private void Update()
    {
        // If the game is not paused
        if (!isPaused)
        {
            // Checks if primary mouse button is down
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hitInfo = new RaycastHit();
                
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Factory")
                {
                    if (currentlySelectedFactory != null)                                           // Checks if a building is already selected
                    {
                        currentlySelectedFactory.ActivateGUI(false);                                // Disables menu of previously selected game object
                    }
                    currentlySelectedFactory = hitInfo.transform.GetComponent<FactoryBuilding>();   // Sets newly selected game object

                    currentlySelectedFactory.ActivateGUI(true);                                     // Activates GUI of game object
                }
                else if (currentlySelectedFactory != null)
                {
                    currentlySelectedFactory.ActivateGUI(false);                                    // Disables menu of previously selected game object
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        optionsMenu.enabled = false;
        Cursor.lockState = CursorLockMode.None;
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
        // Freezes rotation while paused, needed to fix a bug causing major rotation sometimes, TODO: Find and fix that bug, then remove the following line of code
        Camera.main.transform.GetComponent<Rigidbody>().freezeRotation = !active;

        // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
        Cursor.visible = active || inputManager.frozenAngle; // WIP
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
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
}
