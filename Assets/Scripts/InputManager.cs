using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{

    public enum Actions
    {
        FORWARDS,
        BACKWARDS,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        ZOOM
    };

    public KeyCode[] bindings;
    
    // Initial keyInput values, run on startup
    public InputManager()
    {
        bindings = new KeyCode[System.Enum.GetValues(typeof(Actions)).Length];

        bindings[(int)Actions.FORWARDS] = getKeyCodeFromPlayerPrefs(Actions.FORWARDS, KeyCode.W);
        bindings[(int)Actions.LEFT] = getKeyCodeFromPlayerPrefs(Actions.LEFT, KeyCode.A);
        bindings[(int)Actions.BACKWARDS] = getKeyCodeFromPlayerPrefs(Actions.BACKWARDS, KeyCode.S);
        bindings[(int)Actions.RIGHT] = getKeyCodeFromPlayerPrefs(Actions.RIGHT, KeyCode.D);
        bindings[(int)Actions.UP] = getKeyCodeFromPlayerPrefs(Actions.DOWN, KeyCode.Space);
        bindings[(int)Actions.DOWN] = getKeyCodeFromPlayerPrefs(Actions.DOWN, KeyCode.LeftShift);
        bindings[(int)Actions.ZOOM] = getKeyCodeFromPlayerPrefs(Actions.ZOOM, KeyCode.LeftControl);
    }

    // Changes selected control to the specified key-code
    public void changeControl(Actions action, KeyCode keyCode)
    {
        bindings[(int)action] = getKeyCodeFromPlayerPrefs(action, keyCode);
    }

    private KeyCode getKeyCodeFromPlayerPrefs(Actions action, KeyCode defaultValue)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(System.Enum.GetName(typeof(Actions), action), System.Enum.GetName(typeof(KeyCode), defaultValue)));
    }
}
