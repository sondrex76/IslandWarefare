using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static InputManager inputManager;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Stops object from being destroyed

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Insert code here
    }
}
