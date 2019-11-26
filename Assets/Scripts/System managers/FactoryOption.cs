using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Option for factory production
public class FactoryOption : MonoBehaviour
{
    Resource resource;                                      // Resource
    [SerializeField] Slider slider;                         // Slider contianing number of resource you want to produce
    [SerializeField] Text sliderText;                       // Text of slider
    [SerializeField] GameObject textElement;                // Spawnable text element
    [SerializeField] Text buttonText;                       // Text on button

    List<Text> resourceSpecigyingTexts = new List<Text>();  // List of text objects containing needed data
    List<int> resourceIndex = new List<int>();              // Index of equivalent resources

    // Initializes factory options, cnanot be awake since resources must be sent
    public void InitializeOption(Resource r, RectTransform parentPanel)
    {

        resource = r;
        // Sets parent not to be parent to make it have base coordinates at the middle of the screen
        transform.SetParent(transform.parent, false);

        // Sets text of button to be that of the resource's name
        // buttonText.text = r.name;

        // Goes through all parent resources
        // Will not currently handle more then two parent resources well
        int i = 0;
        foreach (Resource.ResourceAmount newResoruce in resource.ReturnParents())
        {
            // Initializes text of resource
            // TODO: make the text apepar at correct location
            GameObject textObject = (GameObject)Instantiate(textElement);
            textObject.transform.SetParent(parentPanel, true);
            textObject.transform.localScale = new Vector3(1, 1, 1);
            textObject.transform.localPosition = new Vector3(245, 12 - i * 18, 0);

            // Gets text
            Text txt = textObject.GetComponent<Text>();

            // Adds text to list
            resourceSpecigyingTexts.Add(txt);

            // Gets elements needed to set the text
            int index = 0;
            for (; index < GameManager.resources.Length; index++)
            {
                // Checks if the right resource has been identified
                if (GameManager.resources[index].resource == newResoruce.resource)
                {
                    // Adds index to list of resource indexes
                    resourceIndex.Add(index);
                    // Disrupts the loop since right index has been found
                    break;
                }
            }
            float currentResourcAmount = GameManager.resources[index].amount;
            float neededResourceAmount = newResoruce.amount;
            // Sets text of element
            txt.text = "(" + currentResourcAmount + "/" + neededResourceAmount + ") " + newResoruce.resource.name;
            i++;
        }
    }


    void FixedUpdate()
    {
        // Updates resources text
        for (int i = 0; i < resourceIndex.Count; i++)
        {
            float currentResourcAmount = GameManager.resources[resourceIndex[i]].amount;
            float neededResourceAmount = resource.ReturnParents()[i].amount;
            resourceSpecigyingTexts[i].text = "(" + currentResourcAmount + "/" + neededResourceAmount + ") " + resource.ReturnParents()[i].resource.name;
        }

        // Checks if resources is empty or not
        if (resource != null)
        {
            // Updates value indicating number of element to be produced
            int numProduce = (int)slider.value;
            // Sets slider text and adds a 0 before the number if it is smaller than 10
            sliderText.text = slider.value.ToString("00");
        }
    }

    // Updates isInGUI in game manager
    public void UpdateFactoryBusy(bool currentState)
    {
        Debug.Log(currentState); // DEBUG
        GameManager.isInGUI = currentState;
    }
}
