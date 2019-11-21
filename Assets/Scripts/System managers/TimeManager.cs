using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    int currentSpeed = 1;
    Image previousButton;
    [SerializeField] GameObject[] timeButtons;  // Buttons


    private void Start()
    {
        // Sets default speed button to green color and sets the previous button as it
        timeButtons[currentSpeed].GetComponent<Image>().color = Color.green;
        previousButton = timeButtons[currentSpeed].GetComponent<Image>();
    }

    // Updates speed, using the name of the game object here named speed
    public void UpdateSpeed(GameObject speed)
    {
        // Sets the rate of speed to a multiplier speed unless it is 0
        // Is used to set speed as 1x, 2x or 3x
        // At 0 the isPaused bool in the game manager is set
        
        // Gets image of button being pressed
        Image currentButton = speed.GetComponent<Image>();

        // Resets previously clicked button if it exists
        previousButton.color = Color.white;

        // Checks which button were clicked and responds accordingly
        if (speed.name != "" + 0)
        {
            // Sets the previous button to the current one
            previousButton = currentButton;

            // Sets background of button to be colored green
            currentButton.color = Color.green;
            // Sets the pause button to white in case it was clicked previously
            timeButtons[0].GetComponent<Image>().color = Color.white;

            // Updates relevant values
            Time.timeScale = currentSpeed = Int32.Parse(speed.name);
            GameManager.isPaused = false;
            currentSpeed = Int32.Parse(speed.name);
        }
        else if (currentSpeed == 0) // Stop has been called but The game were already "paused", set speed to previous one
        {
            GameManager.isPaused = false;
            // Sets speed to that of the button previously clicked
            Time.timeScale = currentSpeed = Int32.Parse(previousButton.name);

            // Sets background color of preivous time to white
            timeButtons[Int32.Parse(previousButton.name)].GetComponent<Image>().color = Color.green;
            timeButtons[0].GetComponent<Image>().color = Color.white;
        }
        else                        // Stop has been called
        {
            // Sets background of button to be colored green
            currentButton.color = Color.green;

            // Freezes values and sets timescale to 1 in case button is clicked again
            Time.timeScale = 1;
            GameManager.isPaused = true;
            currentSpeed = 0;
        }
    }
}
