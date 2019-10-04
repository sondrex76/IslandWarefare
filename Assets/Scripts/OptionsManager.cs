using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    // Updates specific button's text
    public void UpdateButtonText(KeyCode key, GameObject currentButton)
    {
        // Text element with new text of selected button
        string newText = "";

        // Checks which button has been selected and updates text element
        switch (currentButton.name)
        {
            case "ForwardsInput":
                newText = "Forwards";
                break;
            case "BackwardsInput":
                newText = "Backwards";
                break;
            case "LeftInput":
                newText = "Left";
                break;
            case "RightInput":
                newText = "Right";
                break;
            case "UpInput":
                newText = "Up";
                break;
            case "DownInput":
                newText = "Down";
                break;
            case "ZoomInput":
                newText = "Zoom";
                break;
        }

        newText += "(" + key + ")"; // Adds the key for current button to the base text
        currentButton.transform.GetComponentInChildren<Text>().text = newText;
    }
}
