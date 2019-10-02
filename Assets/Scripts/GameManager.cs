using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static InputManager inputManager;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (inputManager == null)
        {
            inputManager = new InputManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.FORWARDS]))  // FORWARDS
        {
            Debug.Log("FORWARDS!");
        }
    }
}
