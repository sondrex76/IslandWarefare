using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Option for factory production
public class FactoryOption : MonoBehaviour
{
    Resource resource;                              // Resource
    [SerializeField] Slider slider;                 // Slider contianing number of resource you want to produce
    [SerializeField] Text sliderText;               // Text of slider
    [SerializeField] GameObject textElement;        // Spawnable text element

    // Initializes factory options, cnanot be awake since resources must be sent
    public void InitializeOption(Resource r)
    {
        resource = r;
        transform.SetParent(transform.parent, false);
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

            // Goes through all parent resources
            // Will not currently handle more then two parent resources well
            int i = 0;
            foreach (Resource.ResourceAmount r in resource.ReturnParents())
            {
                // Initializes text of resource
                GameObject textObject = (GameObject)Instantiate(textElement);
                textObject.transform.position -= new Vector3(0, i * 18, 0);
                // Gets text
                Text txt = textObject.GetComponent<Text>();
                // Gets elements needed to set the text
                int index = 0;
                for (; index < GameManager.resources.Length; index++)
                {
                    // Checks if the right resource has been identified
                    if (GameManager.resources[index].resource == r.resource)
                    {
                        // Disrupts the loop since right index has been found
                        break;
                    }
                    index++;
                }

                float currentResourcAmount = GameManager.resources[index].amount;
                float neededResourceAmount = r.amount;
                // Sets text of element
                txt.text = "(" + currentResourcAmount + "/" + neededResourceAmount + ") " + r.resource.name;
                i++;
            }
        }
    }

    // Updates isInGUI in game manager
    public void UpdateFactoryBusy(bool currentState)
    {
        Debug.Log(currentState); // DEBUG
        GameManager.isInGUI = currentState;
    }
}
