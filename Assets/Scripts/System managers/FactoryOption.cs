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
    [SerializeField] Button button;                         // Button

    List<Text> resourceSpecifyingTexts = new List<Text>();  // List of text objects containing needed data
    List<int> resourceIndex = new List<int>();              // Index of equivalent resources

    FactoryBuilding factoryBuilding;

    // Initializes factory options, cnanot be in awake since resources must be sent
    public void InitializeOption(Resource r, RectTransform parentPanel, FactoryBuilding factory)
    {
        // Sets values sent to the factory
        factoryBuilding = factory;
        resource = r;

        // Sets parent not to be parent to make it have base coordinates at the middle of the screen
        transform.SetParent(transform.parent, false);
        
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
            resourceSpecifyingTexts.Add(txt);

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
            int currentResourcAmount = (int)GameManager.resources[index].amount;
            int neededResourceAmount = (int)newResoruce.amount;
            
            // Sets text of element
            txt.text = "(" + currentResourcAmount + "/" + neededResourceAmount + ") " + newResoruce.resource.ReturnResourceName();
            i++;
        }
    }


    void FixedUpdate()
    {
        // Checks if resources is empty or not
        if (resource != null)
        {
            // Updates value indicating number of element to be produced
            int numProduce = (int)slider.value;
            // Sets slider text and adds a 0 before the number if it is smaller than 10
            sliderText.text = slider.value.ToString("00");

            // Updates resources text
            for (int i = 0; i < resourceIndex.Count; i++)
            {
                int currentResourcAmount = (int)GameManager.resources[resourceIndex[i]].amount;
                int neededResourceAmount = (int)resource.ReturnParents()[i].amount * numProduce;
                resourceSpecifyingTexts[i].text = "(" + currentResourcAmount + "/" + neededResourceAmount + ") " + resource.ReturnParents()[i].resource.ReturnResourceName();

                // Changes color of text based on how much resources are available and eenables/disables ability to buy
                if (currentResourcAmount > neededResourceAmount * 2)        // Enough for more then one purchapse
                {
                    resourceSpecifyingTexts[i].color = Color.green;
                    button.enabled = true;
                } else if (currentResourcAmount >= neededResourceAmount)    // Enough for one purchapse
                {
                    resourceSpecifyingTexts[i].color = Color.yellow;
                    button.enabled = true;
                } else                                                      // Not enough for any purchapse
                {
                    resourceSpecifyingTexts[i].color = Color.red;
                    button.enabled = false;
                }
            }
        }
    }

    // Attempts to buy resource
    public void BuyResource()
    {
        if (!factoryBuilding.ReturnIsBusy())
        { // Checks if the factory is already busy
          // List of amounts, used so calculations need only be done once per button press
            List<int> amount = new List<int>();

            // Goes through all resources
            for (int i = 0; i < resourceIndex.Count; i++)
            {
                // Gathers needed and current resource amounts
                int numProduce = (int)slider.value;
                int currentResourcAmount = (int)GameManager.resources[resourceIndex[i]].amount;
                int neededResourceAmount = (int)resource.ReturnParents()[i].amount * numProduce;
                amount.Add(neededResourceAmount);

                if (currentResourcAmount < neededResourceAmount)   // Checks if there are insufficient amounts of the current resource
                {
                    return;
                }
            }

            // The program has not returned, enough of the resources are present

            // Reduce amount of required resources by the specified amount
            for (int i = 0; i < resourceIndex.Count; i++)
            {
                GameManager.resources[resourceIndex[i]].amount -= amount[i];
            }

            // Adds produced resource
            for (int i = 0; i < GameManager.resources.Length; i++)
            {
                // Checks if the right resource has been identified
                if (GameManager.resources[i].resource == resource)
                {
                    // Set an amount of time before the fabrication is finished
                    factoryBuilding.SetIsBusy(i, GameManager.resources[i].resource.ReturnProductionTime(), (int)slider.value);

                    return; 
                }
            }
            Debug.Log("This line of code should never be reached!");
        }
    }

    // Updates isInGUI in game manager
    public void UpdateFactoryBusy(bool currentState)
    {
        GameManager.isInGUI = currentState;
    }
}
