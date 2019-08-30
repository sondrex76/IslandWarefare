using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Rigidbody cameraBody;
    Camera cameraElement;
    public float cameraSpeed = 10.0f;                                   // Speed of camera
    public float cameraAngleX = 0, cameraAngleY = 0;                    // Angle of camera
    public float horizontalAngularSpeed = 1, verticalAngularSpeed = 1;  // Rotation speed of camera
    public bool reverseHorizontal = false;
    // float previousTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraBody = GetComponent<Rigidbody>();
        cameraElement = GetComponent<Camera>();
    }

    // Gets inout keys and updates position
    void updatePosition()
    {
        cameraBody.velocity = 0 * transform.right; // sets speed to 0 as the base

        // Key detection for movement
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))    // Right
        { // Right
            cameraBody.velocity += cameraSpeed * transform.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))     // Left
        {
            cameraBody.velocity -= cameraSpeed * transform.right;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))       // Forward
        {
            cameraBody.velocity += cameraSpeed * transform.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))     // Backward
        {
            cameraBody.velocity -= cameraSpeed  * transform.forward;
        }
        if (Input.GetKey(KeyCode.Space))                                    // Up
        {
            cameraBody.velocity += cameraSpeed * transform.up;
        }
        if (Input.GetKey(KeyCode.LeftShift))                                // Down
        {
            cameraBody.velocity -= cameraSpeed * transform.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraAngleX += Input.GetAxis("Mouse X");
        cameraAngleY += reverseHorizontal ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y");
        cameraElement.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0); // y, x, z

        updatePosition(); // Updates position of the camera
    }
}
