using UnityEngine;

// Requires MainMenu to have run first
public class CameraMovement : MonoBehaviour
{
    InputManager inputManager;                                                              // Input manager
    GameManager gameManager;                                                                // Game manager

    Rigidbody cameraBody;
    Camera cameraElement;
    [SerializeField] float cameraSpeed = 10.0f;                                             // Speed of camera
    [SerializeField] float cameraAngleX = 0, cameraAngleY = 0;                              // Angle of camera
    [SerializeField] float cameraZoom = 90.0f;                                              // Zoom of camera
    [SerializeField] float zoomSpeed = 2.0f;                                                // Speed of scrolling
    [SerializeField] float horizontalAngularSpeed = 1, verticalAngularSpeed = 1;            // Rotation speed of camera
    [SerializeField] float minimumZoom = 50.0f, maximumZoom = 150.0f;                       // Defiens maximum and minimum zoom
    [SerializeField] float minimumHeight = 5.0f, maximumHeight = 12.0f;                     // Defiens maximum and minimum height
    [SerializeField] float minimumVerticalTilt = -20, maximumVerticalTilt = 20;             // Defiens minimum and mazimum vertical tilt
    [SerializeField] float zoomValue = 25.0f;                                               // Added zoom from zoom button(ctrl)
    [SerializeField] bool reverseVertical = false;                                          // Bool for determining if vertical mvoement should be inverted
    // float previousTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initializes some values
        cameraBody = GetComponent<Rigidbody>();
        cameraElement = GetComponent<Camera>();
        inputManager = GameManager.inputManager;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Zooms the camera depending on the user's wishes
    void updateZoom() {
        // Updates value of zoom
        float scrollValue = cameraZoom - Input.mouseScrollDelta.y * zoomSpeed;
        
        // Updates zoom value and ensures it does not go above maximum or below minimum
        if (scrollValue > maximumZoom)
            scrollValue = maximumZoom;
        else if (scrollValue < minimumZoom)
            scrollValue = minimumZoom;
        
        cameraZoom = scrollValue; // updates camera zoom value so it does not get reset with the next update

        // Zooms if you are clicking the zoom key
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.ZOOM]))
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
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.FORWARDS]))                   // FORWARDS
        {
            cameraBody.velocity += cameraSpeed * transform.forward;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.LEFT]))                       // LEFT
        {
            cameraBody.velocity -= cameraSpeed * transform.right;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.RIGHT]))                      // RIGHT
        {
            cameraBody.velocity += cameraSpeed * transform.right;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.BACKWARDS]))                  // BACKWARDS
        {
            cameraBody.velocity -= cameraSpeed * transform.forward;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.UP]) &&                       // UP
            cameraBody.position.y < maximumHeight)
        {
            cameraBody.velocity += cameraSpeed * transform.up;
        }
        if (Input.GetKey(inputManager.bindings[(int)InputManager.Actions.DOWN]) &&                     // DOWN
            cameraBody.position.y > minimumHeight)
        {
            cameraBody.velocity -= cameraSpeed * transform.up;
        }

        if (Input.GetKeyDown(inputManager.bindings[(int)InputManager.Actions.CHANGE_CAMERA_MODE])) {   // Turn camera angle on or off
            inputManager.frozenAngle = !inputManager.frozenAngle;

            // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
            Cursor.visible = GameManager.isPaused || inputManager.frozenAngle;
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        // Resets vertical angle
        cameraElement.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateZoom();                                      // Updates the zoom of the camera
        updatePosition();                                  // Updates position of the camera
        if (!inputManager.frozenAngle) updateRotation();   // Updates rotation of the camera
    }
}
