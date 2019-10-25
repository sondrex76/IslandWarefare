using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static InputManager inputManager;        // The input manager
    public bool isPaused = false;                   // Is paused

    // Resources
    public int resourceMoney = 0;
    public int resourceOil = 0;
    public int resourceFood = 0;

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
    }
    
    // Updates canvas to being active or inactive
    public void UpdateCanvas(bool active)
    {
        optionsMenu.enabled = isPaused = active;
    }

    // Sets system to expect an action's input to be changed
    public void UpdateInputKey(int selectedAction)
    {
        inputManager.updateSelectedAction(selectedAction);
        optionsManager.currentButton = EventSystem.current.currentSelectedGameObject;
    }
}
