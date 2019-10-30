using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static InputManager inputManager;        // The input manager
    public bool isPaused = false;                   // Is paused

    // Resources, resources[i] and resourceAmounts[i] are for the same object
    public Resource[] resources;
    public float[] resourceAmounts;

    // Amount of money
    public float moneyAmount = 0;
    

    // Options
    [SerializeField] Canvas optionsMenu;            // The options menu
    [SerializeField] OptionsManager optionsManager; // The options manager
    
    // Start is called before the first frame update
    void Awake()
    {
        // DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }

        optionsMenu.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }
    
    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        optionsMenu.enabled = isPaused = active;

        // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
        Cursor.visible = active || inputManager.frozenAngle; // WIP
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
        optionsManager.currentButton = EventSystem.current.currentSelectedGameObject;
    }
}
