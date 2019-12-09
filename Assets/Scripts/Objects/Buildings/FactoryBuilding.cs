using UnityEngine;
using UnityEngine.UI;

// Factory class
public class FactoryBuilding : AbstractBuilding
{
    [SerializeField] Resource[] producableResources;    // List of producable resources
    [SerializeField] GameObject gui;                    // GUI element
    [SerializeField] Image arrow;                       // Arrow object above building

    // [SerializeField] Material outlneMaterial;        // Material of outline
    
    Color materialColor;
    string outline = "_FirstOutlineColor";

    // Prefabs and parent rect
    [SerializeField] GameObject prefabOption;
    // [SerializeField] GameObject prefabProgressBar;
    [SerializeField] RectTransform parentPanel;

    // Values related to factory production
    [SerializeField]float remainingTimeSec = 0;         // Time until the building is no longer busy
    float timePerRound = -1;                            // Amount of time per round of resource generation
    int resourceProducedIndex = -1;                     // Index of resource being produced
    int remainingRounds = 0;                            // Number of rounds of reosurce geeneration which remains
    int originalNumRounds = 0;                          // Original value of remaining rounds for this cycle, needed for progress bar
    bool isBusy = false;                                // Bool indicating if the factory is busy or not

    [SerializeField] Slider sliderProgressBar;          // Slider of progress bar
    [SerializeField] GameObject parentObjectSlider;     // Parent object for slider

    // Code to run at start after initialization code in AbstractBuilding
    private void Start()
    {
        // Makes arrow point stright downwards
        arrow.transform.eulerAngles = Vector3.zero;
        // Makes the arro be right above the building
        arrow.transform.position = new Vector3(transform.position.x, transform.position.y + 45, transform.position.z);

        if (gui == null)                    // Checks if GUI have already been set
            gui = GameObject.Find("GUI");   // Sets GUI
        
        // Sets activation to disabled
        ActivateGUI(false);

        // Claculates offset
        float posOffset = -producableResources.Length / 2.0f * 43;

        // Sets up slider
        parentObjectSlider.transform.SetParent(parentPanel, true);
        parentObjectSlider.transform.localPosition = Vector3.zero;
        parentObjectSlider.transform.localPosition = new Vector3(132, -posOffset - 10);

        // Generate GUI
        for (int i = 0; i < producableResources.Length; i++)
        {
            GameObject optionGUI_Element = (GameObject)Instantiate(prefabOption);
            optionGUI_Element.transform.SetParent(parentPanel, true);
            optionGUI_Element.transform.localScale = new Vector3(1, 1, 1);

            optionGUI_Element.transform.position = new Vector3(0, posOffset + i * 43, 0);

            // Gets RectTransform of canvas of child
            RectTransform rectTransform = optionGUI_Element.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();

            // Initialzes values and sends current resource over
            optionGUI_Element.GetComponent<FactoryOption>().InitializeOption(producableResources[i], rectTransform, this);
            
            // Modifies text
            optionGUI_Element.GetComponentInChildren<Text>().text = producableResources[i].ReturnResourceName();
        }
    }

    // Code to be run on fixedUpdate
    override protected void BuildingFunctionality()
    {
        if (arrow.enabled) // if the arrow is enabled
        {
            arrow.transform.LookAt(Camera.main.transform.position);
        }
        
        // Checks if factory is busy
        if (isBusy)
        {
            // Updates remaining time
            remainingTimeSec -= Time.deltaTime;

            // Updates slider for progress bar
            sliderProgressBar.value = ((timePerRound - remainingTimeSec) + timePerRound * (originalNumRounds - remainingRounds)) / (timePerRound * originalNumRounds);

            // Checks if time limit has been reached
            if (remainingTimeSec <= 0)
            {
                Debug.Log(GameManager.resources[resourceProducedIndex].amount); // DEBUG

                // Checks if more rounds remain and updates remainingRounds
                if (--remainingRounds > 0)
                {
                    remainingTimeSec = timePerRound;
                    GameManager.resources[resourceProducedIndex].amount++;
                }
                else
                {
                    GameManager.resources[resourceProducedIndex].amount++;
                    isBusy = false;
                    resourceProducedIndex = -1;
                    timePerRound = -1;
                }
            }
        }
    }

    // Activates/deactivates GUI
    public bool ActivateGUI(bool activate)
    {
        // Checks if the building is finished
        if (finishedBuilding)
        {
            // Activates GUI
            gui.SetActive(activate);
            arrow.enabled = activate;
        }
        // If the building is not complete it disables the menu and sets outline to not be colored
        else
        {
            gui.SetActive(false);
            arrow.enabled = false;
        }

        return finishedBuilding;
    }

    // Returns if whether factory is busy or not
    public bool ReturnIsBusy()
    {
        return isBusy;
    }

    // Makes factory unable to produce anything until the time sent is finished, and specifies what resource to produce
    public void SetIsBusy(int index, float timeBusy = 0, int numRounds = 1, bool busy = true)
    {
        resourceProducedIndex = index;                      // Updates index
        timePerRound = remainingTimeSec = timeBusy;         // Updates both current countdown and the time value per countdown
        remainingRounds = originalNumRounds = numRounds;    // Updates number of remaining rounds
        isBusy = busy;                                      // Sets isBusy to true
    }


    // LoadFactory code, must be run when loading factory
    public void LoadFactory(bool isWorking = false, float rTime = 0, float tRound = 0, int resourceIndex = 0, int rounds = 0, int origRounds = 0)
    {
        isBusy = isWorking;
        // Checks if the building is in the process of producing
        if (isBusy)
        {
            // Updates values
            remainingTimeSec = rTime;
            timePerRound = tRound;
            resourceProducedIndex = resourceIndex;
            remainingRounds = rounds;
            originalNumRounds = origRounds;
        }
    }

    // Returns datavalues of class for use in saving
    public FactorySave ReturnFactorySave(Vector3 position, Vector3 rotation)
    { // bool finished, Vector3 position, Vector3 rotation
        BuildingSave building = ReturnBuildingSave(position, rotation);
        return new FactorySave(
            isBusy, remainingTimeSec, timePerRound, resourceProducedIndex, remainingRounds, originalNumRounds,  // Factory
            building.presentHealth, building.yOffset, building.buildingFinished,                                // Building
            transform.name, position, rotation                                                                  // Transform
            );
    }

    protected override void MakeNode()
    {
        // Move code below
        ObjectPool pool = GameObject.FindGameObjectWithTag("Manager").GetComponent<ObjectPool>();
        GameObject graph = GameObject.FindGameObjectWithTag("Graph");
        GameObject node = pool.GetPooledObject("OfficeNode");

        node.transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y), transform.position.z);
        node.transform.parent = graph.transform;
        node.SetActive(true);
        graph.GetComponent<Graph>().AddNodes();
    }
}
