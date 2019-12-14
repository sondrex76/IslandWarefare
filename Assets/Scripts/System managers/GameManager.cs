using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Public variables
    public static InputManager inputManager;        // The input manager
    public static bool isPaused = false;            // Is paused

    // Array of resources
    public static Resource.ResourceAmount[] resources;

    // Miltary mght, might be expanded upon later
    public static int defensivePower = 0;
    public static int attackPower = 0;
    public static int supplyPower = 0;

    // Amount of resources generated through various systems
    public static float moneyAmount = 0;            // Money
    public static uint population = 0;              // Population
    public static float happiness = 0;              // Happiness, might be changed to be a value between 0 and 100 in the future

    // Bools for states
    public static bool isInGUI;                     // Specifies that the user is in a GUI and it should not be shut down

    public static int numHouses = 0;                // Number of houses
    [SerializeField] float houseMoneyRate = 0.2f;   // Production rate of resources from houses per house per second
    
    public static int numFactories = 0;             // Number of houses
    [SerializeField] float factoryMoneyRate = 1.5f; // Production rate of resources from houses per house per second

    public static int numGatherers = 0;             // Number of houses
    [SerializeField] float gatherMoneyRate = 2.0f; // Production rate of resources from houses per house per second

    [SerializeField] Image arrow;                   // Arrow to be placed above buildings


    float previousTimeSpeed = 1;                    // Previous speed of time
    bool previouslyFrozen = true;                   // Bool used to freeze camera when in pause menu

    // Options
    [SerializeField] Canvas optionsMenu;            // The options menu
    [SerializeField] OptionsManager optionsManager; // The options manager

    FactoryBuilding currentlySelectedFactory;       // Currently selected factory
    bool factorySelected = false;

    Graph graph;
    

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
        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        GetUserData();
    }

    private void Update()
    {
        // If the game is not paused
        if (!isPaused)
        {
            // Increases amount of money based on how much time have passed since the last update and number of houses
            moneyAmount += Time.deltaTime * Time.timeScale * numHouses * houseMoneyRate;

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
                else if (currentlySelectedFactory != null && !isInGUI && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())                              // Checks if there is a factory there and that you are outside of any relevant GUI element
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
        }

        if (factorySelected) // checks if the user is in a relevant GUI(Factory)
        {
            Debug.Log(currentlySelectedFactory.transform.position.y);

            // Make the arrow always face the player while active
            arrow.transform.LookAt(new Vector3(Camera.main.transform.position.x, currentlySelectedFactory.transform.position.y + 45, Camera.main.transform.position.z));
        }

        if (Time.frameCount % 500 == 0 && graph != null)
        {
            uint newPop = 0;
            foreach(var node in graph.Nodes.Where(node => node.attribute == GraphNode.Attribute.Residential))
            {
                newPop += (uint)node.GetComponent<ResidentialNode>().citizens.Count;
            }
            population = newPop;
        }
    }
    

    public void TrggerCanvas()
    {
        UpdateCanvas(!isPaused);
    }

    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool inactive)
    {
        optionsMenu.enabled = isPaused = inactive;

        // if paused then it sets speed to zero
        if (inactive)
        {
            previouslyFrozen = inputManager.frozenAngle;
            previousTimeSpeed = Time.timeScale;
            Time.timeScale = 0;
            inputManager.frozenAngle = true;
        }
        else
        {
            Time.timeScale = previousTimeSpeed;
            inputManager.frozenAngle = previouslyFrozen;
        }
        
        // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
        Cursor.visible = inactive || inputManager.frozenAngle; // WIP
        if (Cursor.visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.UpdateSelectedAction(selectedAction);
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

    // Returns mouse mode
    public bool ReturnCameraMode()
    {
        return previouslyFrozen;
    }

    //Get the the diffrent powers from server
    void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, SetPowers, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    //Set the powers gotten from server
    void SetPowers(GetUserDataResult result) 
    {
        attackPower = Int32.Parse(result.Data["AttackPower"].Value);
        defensivePower = Int32.Parse(result.Data["DefencePower"].Value);
        attackPower = Int32.Parse(result.Data["SupportPower"].Value);
    }

    //Add the powers when buying new units
    public void AddPower(MilitaryUnit unit)
    {
        if (moneyAmount >= unit.cost)
        {
            attackPower += unit.attackPower;
            defensivePower += unit.defencePower;
            supplyPower += unit.supplyPower;

            moneyAmount -= unit.cost;

            //Update the powers in database
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>() {
                    {"AttackPower", attackPower.ToString()},
                    {"DefencePower", defensivePower.ToString()},
                    {"SupportPower", supplyPower.ToString()}
                }
            },
              result => Debug.Log("Successfully updated user data"),
              error =>
              {
                  Debug.Log("Got error setting user data Ancestor to Arthur");
                  Debug.Log(error.GenerateErrorReport());
              });

        }
    }

    public void BuyBuilding(int cost)
    {
        moneyAmount -= cost;
    }


    public float GetMoneyAmount()
    {
        return moneyAmount;
    }



}
