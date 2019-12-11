using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ResourceGuiManager : MonoBehaviour
{
    // GUI elements
    [SerializeField] Canvas currentCanvas;
    [SerializeField] Canvas parentCanvas;
    [SerializeField] GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] resourceObjects = Resources.LoadAll("Prefabs/WorldResources/Raw resources").Cast<GameObject>().ToArray();
        
        // Data
        Color[] colors = new Color[resourceObjects.Length];
        string[] names = new string[resourceObjects.Length];
        
        // Loads name and color of resources
        for (int i = 0; i < resourceObjects.Length; i++)
        {
            colors[i] = resourceObjects[i].GetComponentInChildren<ColorChange>().ReturnColor();
            names[i] = resourceObjects[i].GetComponent<Resource>().ReturnResourceName();

            // Spawns GUI element and sets color and text
            GameObject currentObject = Instantiate(prefab);
            currentObject.transform.SetParent(currentCanvas.transform, true);


            // Updates values for color and text
            currentObject.GetComponentInChildren<Text>().text = names[i];
            currentObject.GetComponent<Image>().color = new Vector4(colors[i].r, colors[i].g, colors[i].b, 1);
        }

        // Disables GUI
        ToggleGUI(parentCanvas);
    }
    
    // Toggles parent Canvas
    public void ToggleGUI(Canvas currentCanvas)
    {
        currentCanvas.enabled = !currentCanvas.enabled;
    }
}
