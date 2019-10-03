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

    public void updateCanvas(bool active)
    {
        optionsMenu.enabled = active;

    }

    // Update is called once per frame
    void Update()
    {
        // Insert code here
    }
}
