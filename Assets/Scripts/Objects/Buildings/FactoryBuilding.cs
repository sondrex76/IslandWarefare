using UnityEngine;

// Factory class
public class FactoryBuilding : AbstractBuilding
{
    [SerializeField] Resource[] producableResources;    // List of producable variables
    [SerializeField] GameObject gui;                    // GUI element
    [SerializeField] MeshRenderer shaders;              // Shaders of object
    [SerializeField] Material outlneMaterial;           // Material of outline

    Renderer materialRenderer;

    Color materialColor;
    string outline = "_FirstOutlineColor";

    // Code to run at start after initialization code in AbstractBuilding
    private void Start()
    {
        if (gui == null)                    // Checks if GUI have already been set
            gui = GameObject.Find("GUI");   // Sets GU

        // Finds the meshRenderer
        shaders = building.GetComponent<MeshRenderer>();
        materialColor = shaders.materials[1].GetColor(outline);
        
        // Sets the renderer
        materialRenderer = gameObject.GetComponentInChildren<Renderer>();

        // Sets activation to disabled
        ActivateGUI(false);                 
    }

    // Code to be run on fixedUpdate
    override protected void BuildingFunctionality()
    {
        // TODO add code
    }

    // Activates/deactivates GUI
    public bool ActivateGUI(bool activate)
    {
        // Checks if the building is finished
        if (finishedBuilding)
        {
            // Activates GUI
            gui.SetActive(activate);

            // Checks if it should activate or deactivate the outline
            if (activate)
            {
                // Sets outline color back to default
                materialRenderer.materials[1].SetColor(outline, materialColor);
            }
            else
            {
                // Hides outline, TODO: completely disable instead of just making it invisible
                materialRenderer.materials[1].SetColor(outline, new Color(0, 0, 0, 0));
            }
        }
        // If the building is not complete it disables the menu and sets outline to not be colored
        else
        {
            materialRenderer.materials[1].SetColor(outline, new Color(0, 0, 0, 0));
            gui.SetActive(false);
        }

        return finishedBuilding;
    }
}
