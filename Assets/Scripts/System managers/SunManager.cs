using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    [SerializeField] Light sunObject;           // Sun
    [SerializeField] float timeRotationSeconds; // Time to rotate object

    float timeValue;
    private void Start()
    {
        timeValue = 360 / 50 / timeRotationSeconds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If game manager is not paused the sun will be moved
        if (!GameManager.isPaused)
        {
            sunObject.transform.Rotate(timeValue, 0, 0);
        }   
    }
}
