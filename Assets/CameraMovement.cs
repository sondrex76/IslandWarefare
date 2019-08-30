using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Rigidbody cameraBody;
    public float cameraSpeed = 10.0f;   // Base speed of camera
    // float previousTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraBody.velocity = 0 * transform.right; // sets speed to 0 as the base

        // Key detection for movement
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) { // Right
            cameraBody.velocity = cameraSpeed * transform.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) // Left
        {
            cameraBody.velocity = cameraSpeed * -transform.right;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) // Up
        {
            cameraBody.velocity = new Vector3(1, 0, 0) * cameraSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) // Down
        {
            cameraBody.velocity = new Vector3(-1, 0, 0) * cameraSpeed;
        }
        if (Input.GetKey(KeyCode.Space)) // Up
        {
            cameraBody.velocity = new Vector3(0, 0, 1) * cameraSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift)) // Down
        {
            cameraBody.velocity = new Vector3(0, 0, -1) * cameraSpeed;
        }
    }
}
