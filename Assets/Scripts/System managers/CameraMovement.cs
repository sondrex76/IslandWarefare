using UnityEngine;

// Requires MainMenu to have run first
public class CameraMovement : MonoBehaviour
{
    GameManager gameManager;                                                                // Game manager

    Rigidbody cameraBody;
    Camera cameraElement;
    [SerializeField] float cameraSpeed = 10.0f;                                             // Speed of camera
    [SerializeField] static float cameraAngleX = 0, cameraAngleY = 0;                       // Angle of camera, names based on mouse input axis name
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
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.ZOOM]))
            scrollValue -= zoomValue;

        cameraElement.fieldOfView = scrollValue;
    }

    // Updates rotation of camera
    void updateRotation()
    {
        cameraAngleX += Input.GetAxis("Mouse X");   // Adds horizontal mouse movement


        // Adds input or reversed input depending on if camera has been reversed
        float limitedAngle = cameraAngleY + (reverseVertical ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y"));
        
        // Adds vertical mouse movement if it is neither above or below the maximum, and sets it to max/min if it is above/below
        if (limitedAngle > maximumVerticalTilt)
            limitedAngle = maximumVerticalTilt;
        else if (limitedAngle < minimumVerticalTilt)
            limitedAngle = minimumVerticalTilt;

        cameraAngleY = limitedAngle;
        
        cameraElement.transform.eulerAngles = new Vector3(cameraAngleY, cameraAngleX, 0);
    }

    // Gets inout keys and updates position
    void updatePosition()
    {
        cameraBody.velocity = Vector3.zero; // sets speed to 0 as the base

        // changes vertical angle to 0 temporarily, ensures camera cannot be moved higher then max or lower then minimum
        cameraElement.transform.rotation = Quaternion.Euler(0, cameraAngleX, 0);

        // Key detection for movement
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.FORWARDS]))                   // FORWARDS
        {
            cameraBody.velocity += transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.LEFT]))                       // LEFT
        {
            cameraBody.velocity -= transform.right * Time.deltaTime;
        }
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.RIGHT]))                      // RIGHT
        {
            cameraBody.velocity += transform.right * Time.deltaTime;
        }
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.BACKWARDS]))                  // BACKWARDS
        {
            cameraBody.velocity -= transform.forward * Time.deltaTime;
        }
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.UP]) &&                       // UP
            cameraBody.position.y < maximumHeight)
        {
            cameraBody.velocity += transform.up * Time.deltaTime;
        }
        if (Input.GetKey(GameManager.inputManager.bindings[(int)InputManager.Actions.DOWN]) &&                     // DOWN
            cameraBody.position.y > minimumHeight)
        {
            cameraBody.velocity -= transform.up * Time.deltaTime;
        }
        
        // Normalizes speed
        cameraBody.velocity = cameraBody.velocity.normalized * cameraSpeed;
        // Modifies speed based on timescale
        if (Time.timeScale != 0)
            cameraBody.velocity /= Time.timeScale;

        // Resets vertical angle
        // cameraElement.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0);
        cameraElement.transform.eulerAngles = new Vector3(cameraAngleY, cameraAngleX, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        // Checks if you are trying to change camera mode and the game is not paused
        if (Input.GetKeyDown(GameManager.inputManager.bindings[(int)InputManager.Actions.CHANGE_CAMERA_MODE]) && !GameManager.isPaused)
        {   // Turn camera angle on or off
            GameManager.inputManager.frozenAngle = !GameManager.inputManager.frozenAngle;

            // Makes mouse invisible when moving about but visible and starting centered when in a menu and when selection is activated
            Cursor.visible = /*GameManager.isPaused || */ GameManager.inputManager.frozenAngle;
            if (Cursor.visible)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }

        updateZoom();                                      // Updates the zoom of the camera
        updatePosition();                                  // Updates position of the camera
        // Updates rotation of the camera
        if (!GameManager.inputManager.frozenAngle) updateRotation();   
    }

    // Returns camera mode
    public bool ReturnCameraMode()
    {
        return gameManager.ReturnCameraMode();
    }

    // Function which loads and defines angles
    public void LoadAngles(float x, float y)
    {
        cameraAngleX += y;
        cameraAngleY += x;
    }
}
