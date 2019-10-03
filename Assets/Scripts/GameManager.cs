using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static InputManager inputManager;
    [SerializeField]Canvas optionsMenu;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        // optionsMenu = transform.GetComponent<Canvas>();
        optionsMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.isSelectingInput())    // Checks if a key is currently being rebound
        {
            // Gets the currently clicked button if there are any
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    inputManager.changeControl(inputManager.returnCurrentlySelectedAction(), vKey);
                    inputManager.finishedUpdateKey();
                    break;
                }
            }
        }
        // Insert code here
    }

    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        optionsMenu.enabled = active;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
    }
}
