using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    bool currentlyReSelectingInput = false; // Is currently in the process of rebinding a key
    Actions currentReSelect;                // Key currently being rebounded
    GameObject currentButton;               // Currently selected button

    // Enum of actions
    public enum Actions
    {
        FORWARDS,
        BACKWARDS,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        ZOOM,
        PAUSE
    };

    public KeyCode[] bindings;
    
    // Initial keyInput values, run on startup
    public InputManager()
    {
        bindings = new KeyCode[System.Enum.GetValues(typeof(Actions)).Length];                      // Initializes bindings

        bindings[(int)Actions.FORWARDS] = getKeyCodeFromPlayerPrefs(Actions.FORWARDS, KeyCode.W);
        bindings[(int)Actions.LEFT] = getKeyCodeFromPlayerPrefs(Actions.LEFT, KeyCode.A);
        bindings[(int)Actions.BACKWARDS] = getKeyCodeFromPlayerPrefs(Actions.BACKWARDS, KeyCode.S);
        bindings[(int)Actions.RIGHT] = getKeyCodeFromPlayerPrefs(Actions.RIGHT, KeyCode.D);
        bindings[(int)Actions.UP] = getKeyCodeFromPlayerPrefs(Actions.DOWN, KeyCode.Space);
        bindings[(int)Actions.DOWN] = getKeyCodeFromPlayerPrefs(Actions.DOWN, KeyCode.LeftShift);
        bindings[(int)Actions.ZOOM] = getKeyCodeFromPlayerPrefs(Actions.ZOOM, KeyCode.LeftControl);
        bindings[(int)Actions.PAUSE] = getKeyCodeFromPlayerPrefs(Actions.PAUSE, KeyCode.Escape);
    }

    // Changes selected control to the specified key-code
    public void changeControl(Actions action, KeyCode keyCode)
    {
        bindings[(int)action] = keyCode;
        PlayerPrefs.SetString(action.ToString(), keyCode.ToString());
    }

    // Returns the keyCode version of the string
    public KeyCode StringToKey(string keyCode)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCode);
    }

    // Gets keycode for specified action
    private KeyCode getKeyCodeFromPlayerPrefs(Actions action, KeyCode defaultValue)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(System.Enum.GetName(typeof(Actions), action), System.Enum.GetName(typeof(KeyCode), defaultValue)));
    }

    // Returns true if a keybinding is in the process of being changed
    public bool isSelectingInput()
    {
        return currentlyReSelectingInput;
    }
    
    // Returns currently selected action
    public Actions returnCurrentlySelectedAction() 
    {
        return currentReSelect;
    }

    // Updates currently selected action and sets currentlyReSelectingInput to true
    public void updateSelectedAction(int selectedAction)
    {
        currentReSelect = (Actions)selectedAction;
        currentlyReSelectingInput = true;
    }

    // Updates key when finished
    public void finishedUpdateKey()
    {
        currentlyReSelectingInput = false;
    }
}
