using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllMapView : MonoBehaviour
{

    [SerializeField]
    float speed;
    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameManager.inputManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.BACKWARDS]))
        {
            transform.Translate(transform.forward * speed);
        }


        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.FORWARDS]))
        {
            transform.Translate(transform.forward * -speed);
        }


        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.LEFT]))
        {
            transform.Translate(transform.right * -speed);
        }

        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.RIGHT]))
        {
            transform.Translate(transform.right * speed);
        }

    }
}
