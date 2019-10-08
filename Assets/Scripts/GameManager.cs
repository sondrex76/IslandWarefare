using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static InputManager inputManager;        // The input manager
    public bool isPaused = false;                   // Is paused

    [SerializeField] Canvas _optionsMenu;            // The options menu
    [SerializeField] OptionsManager _optionsManager; // The options manager

    GameObject currentButton;                       // Currently selected button name

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        _optionsMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if a key is currently being rebound
        if (inputManager.isSelectingInput())    
        {
            // Gets the currently clicked button if there are any
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    inputManager.changeControl(inputManager.returnCurrentlySelectedAction(), vKey);
                    inputManager.finishedUpdateKey();

                    _optionsManager.UpdateButtonText(vKey, currentButton);

                    break;
                }
            }
        }
        // Insert code here
    }

    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        _optionsMenu.enabled = active;
        isPaused = active;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
        currentButton = EventSystem.current.currentSelectedGameObject;
    }
}
