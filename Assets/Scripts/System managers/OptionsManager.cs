using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject currentButton;                                            // Currently selected button for input change
    GameManager gameManager;                                                    // The game manager object
    [SerializeField] GameObject buttons;

    // Runs on start
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // Updates button labels
        for (int i = 0; i < GameManager.inputManager.bindings.Length; i++)
        {
            // Gets the correct child, is offset with 2 since the back button and Key mappings string are above the options
            UpdateButtonText(GameManager.inputManager.bindings[i], buttons.transform.GetChild(i + 2).gameObject);
        }
    }

    // Runs every frame
    private void Update()
    {
        if (Input.GetKeyDown(GameManager.inputManager.bindings[(int)InputManager.Actions.PAUSE]))   // Pause
        {
            gameManager.UpdateCanvas(!GameManager.isPaused);   // two bools rather then one because of
        }

        // Input change
        if (GameManager.inputManager.isSelectingInput()) // Checks if a key is currently being rebound
        {
            // Gets the currently clicked button if there are any
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    GameManager.inputManager.changeControl(GameManager.inputManager.returnCurrentlySelectedAction(), vKey);
                    GameManager.inputManager.finishedUpdateKey();

                    UpdateButtonText(vKey, currentButton);

                    break;
                }
            }
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
