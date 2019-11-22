using UnityEngine;

// Factory class
public class FactoryBuilding : AbstractBuilding
{
    [SerializeField] Resource[] producableResources;    // List of producable variables
    [SerializeField] GameObject gui;                    // GUI element

    // Code to run at start after initialization code in AbstractBuilding
    private void Start()
    {
        if (gui == null)
            gui = GameObject.Find("GUI");
        gui.SetActive(false);
    }

    // Code to be run on fixedUpdate
    override protected void BuildingFunctionality()
    {
        // TODO add code
    }

    // Activates/deactivates GUI
    public bool ActovateGUI(bool activate)
    {
        // Checks if the building is finished
        if (finishedBuilding)
        {
            gui.SetActive(activate);
        }
        return finishedBuilding;
    }
}
