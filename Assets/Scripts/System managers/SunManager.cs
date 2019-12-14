using UnityEngine;

public class SunManager : MonoBehaviour
{
    [SerializeField] Light sun;
    [SerializeField] Light moon;

    [SerializeField] float timeRotationSeconds; // Time to rotate object

    float rotationSpeed;
    private void Start()
    {
        rotationSpeed = 360 / 50 / timeRotationSeconds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If game manager is not paused the sun will be moved
        if (!GameManager.isPaused)
        {
            
            sun.transform.RotateAround(Vector3.zero, Vector3.right, rotationSpeed);
            sun.transform.LookAt(Vector3.zero);
            
           // moon.transform.RotateAround(Vector3.zero, Vector3.right, rotationSpeed);
           // moon.transform.LookAt(Vector3.zero);
            
            
            // if the sun is above the horizon
            /*
            if (sun.transform.position.y >= 0)
            {
               // moon.enabled = false;
                sun.enabled = true;
            }
            else
            {
                sun.enabled = false;
             //   moon.enabled = true;
            }
            */
        }   
    }

    // Returns position(for saving)
    public float[] ReturnPosition()
    {
        return new float[] { sun.transform.position.x, sun.transform.position.y, sun.transform.position.z };
    }

    // Updates position(for loading)
    public void UpdatePosition(float[] pos)
    {
        sun.transform.position = new Vector3(pos[0], pos[1], pos[2]);
        sun.transform.LookAt(Vector3.zero);
    }
}
