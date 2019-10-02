using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requires MainMenu to have run first
public class CameraMovement : MonoBehaviour
{
    InputManager inputManager;                                                  // Inpur manager

    Rigidbody cameraBody;
    Camera cameraElement;
    public float cameraSpeed = 10.0f;                                           // Speed of camera
    public float cameraAngleX = 0, cameraAngleY = 0;                            // Angle of camera
    public float cameraZoom = 90.0f;                                            // Zoom of camera
    public float zoomSpeed = 2.0f;                                              // Speed of scrolling
    public float horizontalAngularSpeed = 1, verticalAngularSpeed = 1;          // Rotation speed of camera
    public float minimumZoom = 50.0f, maximumZoom = 150.0f;                     // Defiens maximum and minimum zoom
    public float minimumHeight = 5.0f, maximumHeight = 12.0f;                   // Defiens maximum and minimum height
    public float minimumVerticalTilt = -20, maximumVerticalTilt = 20;           // Defiens minimum and mazimum vertical tilt
    public float zoomValue = 25.0f;                                             // Added zoom from zoom button(ctrl)
    public KeyCode zoomKey = KeyCode.LeftControl, downKey = KeyCode.LeftShift;  // Zoom and down keys, wasd and space are currently locked
    public bool reverseVertical = false;                                        // Bool for determining if vertical mvoement should be inverted
    // float previousTime;


    // Start is called before the first frame update
    void Start()
    {
        cameraBody = GetComponent<Rigidbody>();
        cameraElement = GetComponent<Camera>();
        inputManager = GameManager.inputManager;
    }

    // Zooms the camera depending on the user's wishes
    void updateZoom() {
        float scrollValue = cameraZoom - Input.mouseScrollDelta.y * zoomSpeed; // Updates value of zoom
        
        // Updates zoom value and ensures it does not go above maximum or below minimum
        if (scrollValue > maximumZoom)
            scrollValue = maximumZoom;
        else if (scrollValue < minimumZoom)
            scrollValue = minimumZoom;
        
        cameraZoom = scrollValue; // updates camera zoom value so it does not get reset with the next update

        // Zooms if you are clicking the zoom key
        if (Input.GetKey(zoomKey))
            scrollValue -= zoomValue;

        cameraElement.fieldOfView = scrollValue;
    }

    // Updates rotation of camera
    void updateRotation()
    {
        cameraAngleX += Input.GetAxis("Mouse X");   // Adds horizontal mouse movement

        float limitedAngle = cameraAngleY + (reverseVertical ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y"));
        
        // Adds vertical mouse movement if it is neither above or below the maximum
        if (limitedAngle <= maximumVerticalTilt && limitedAngle >= minimumVerticalTilt)
            cameraAngleY = limitedAngle;   
        
        cameraElement.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0);         // Updates current angle
    }

    // Gets inout keys and updates position
    void updatePosition()
    {
        cameraBody.velocity = 0 * transform.right; // sets speed to 0 as the base

        // changes vertical angle to 0 temporarily, ensures camera cannot be moved higher then max or lower then minimum
        cameraElement.transform.rotation = Quaternion.Euler(0, cameraAngleX, 0);


        // Key detection for movement
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.FORWARDS]))                // FORWARDS
        {
            cameraBody.velocity += cameraSpeed * transform.forward;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.LEFT]))                    // LEFT
        {
            cameraBody.velocity -= cameraSpeed * transform.right;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.RIGHT]))                   // RIGHT
        {
            cameraBody.velocity += cameraSpeed * transform.right;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.BACKWARDS]))               // BACKWARDS
        {
            cameraBody.velocity -= cameraSpeed * transform.forward;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.UP]))                      // UP
        {
            cameraBody.velocity += cameraSpeed * transform.up;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.DOWN]))                    // DOWN
        {
            cameraBody.velocity -= cameraSpeed * transform.up;
        }


        // Resets vertical angle
        cameraElement.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0);
    }

    // Update is called once per frame
    void Update()
    {
        updateZoom();       // Updates the zoom of the camera
        updateRotation();   // Updates rotation of the camera
        updatePosition();   // Updates position of the camera
    }
}
