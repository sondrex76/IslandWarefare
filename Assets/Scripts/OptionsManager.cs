﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    InputManager inputManager;                                                  // Inpur manager
    [SerializeField] GameManager _gameManager;                                    // The game manager object
    [SerializeField] GameObject _buttons;

    // Runs on start
    private void Start()
    {
        inputManager = GameManager.inputManager;

        // Updates button labels
        for (int i = 0; i < inputManager.bindings.Length; i++)
        {
            // Gets the correct child, is offset with 2 since the back button and Key mappings string are above the options
            UpdateButtonText(inputManager.bindings[i], _buttons.transform.GetChild(i + 2).gameObject);
        }
    }

    // Runs every frame
    private void Update()
    {
        if (Input.GetKeyDown(inputManager.bindings[(int)InputManager.Actions.PAUSE]))   // Pause
        {
            _gameManager.UpdateCanvas(!_gameManager.isPaused);   // two bools rather then one because of
        }
    }

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
            case "PauseInput":
                newText = "Pause";
                break;
            case "ChangeCameraModeInput":
                newText = "Change camera mode";
                break;
        }

        newText += "(" + key + ")"; // Adds the key for current button to the base text
        currentButton.transform.GetComponentInChildren<Text>().text = newText;
    }
}